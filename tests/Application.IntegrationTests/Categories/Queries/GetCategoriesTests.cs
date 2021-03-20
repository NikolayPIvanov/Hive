using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hive.Application.Categories.Queries.GetCategories;
using Hive.Domain.Entities;
using Hive.Domain.Entities.Categories;
using NUnit.Framework;

namespace Hive.Application.IntegrationTests.Categories.Queries
{
    using static Testing;
    
    public class GetCategoriesTests : TestBase
    {
        [Test]
        [TestCase(10, null, false)]
        [TestCase(10, null, true)]
        [TestCase(10, 2, false)]
        [TestCase(10, 2, true)]
        public async Task ShouldReturnCategories(int count, int? limit, bool onlyParent)
        {
            static int Action(List<Category> categories)
            {
                var random = new Random();
                var index = random.Next(0, categories.Count);
                return categories[index].Id;
            }

            var categories = Enumerable.Range(1, count)
                .Select(x => new Category
                {
                    Title = Guid.NewGuid().ToString()
                });
            
            var enumerable = categories.ToList();
            await AddBulkAsync(enumerable);
            
            var child = Enumerable.Range(1, count)
                .Select(x => new Category
                {
                    Title = Guid.NewGuid().ToString(),
                    ParentCategoryId = Action(enumerable)
                });
            var entities = child.ToList();
            await AddBulkAsync(entities);
            
            var query = new GetCategoriesQuery()
            {
                Limit = limit,
                OnlyParent = onlyParent
            };

            var result = await SendAsync(query);

            var expected = enumerable.Concat(entities);
            if (limit.HasValue)
            {
                expected = expected.Take(limit.Value);
            }

            if (onlyParent)
            {
                expected = expected.Where(c => c.ParentCategoryId == null);
            }

            result.Should().HaveSameCount(expected);
        }
    }
}