using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Hive.Application.Categories.Commands.CreateCategory;
using Hive.Application.Categories.Commands.UpdateCategory;
using Hive.Application.Common.Exceptions;
using Hive.Domain.Entities.Gigs;
using NUnit.Framework;

namespace Hive.Application.IntegrationTests.Categories.Commands
{
    using static Testing;
    
    public class UpdateCategoryTests : TestBase
    {
        [Test]
        public void ShouldRequireValidCategoryId()
        {
            var command = new UpdateCategoryCommand()
            {
                Id = 99,
                Title = "Test"
            };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }
        
        [Test]
        public async Task ShouldUpdateCategory()
        {
            var userId = await RunAsDefaultUserAsync();

            var categoryId = await SendAsync(new CreateCategoryCommand
            {
                Title = "Test"
            });
            
            var childCategoryId = await SendAsync(new CreateCategoryCommand()
            {
                Title = "Test Child",
                ParentId = categoryId
            });

            var sideCategoryId = await SendAsync(new CreateCategoryCommand()
            {
                Title = "Side Child"
            });
            
            var command = new UpdateCategoryCommand()
            {
                Id = categoryId,
                Title = "Test Updated",
                SubCategoriesIds = new List<int>() { sideCategoryId }
            };

            await SendAsync(command);

            var item = await FindAsync<Category>(categoryId);
            var childItem =  await FindAsync<Category>(childCategoryId);
            var sideItem =  await FindAsync<Category>(sideCategoryId);
            
            item.Title.Should().Be("Test Updated");
            item.ParentCategoryId.Should().BeNull();
            item.LastModifiedBy.Should().NotBeNull();
            item.LastModifiedBy.Should().Be(userId);
            item.LastModified.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, 10000);

            sideItem.ParentCategoryId.Should().Be(categoryId);
            childItem.ParentCategoryId.Should().BeNull();
        }
        
        [Test]
        public async Task ShouldUpdateChildCategory()
        {
            var userId = await RunAsDefaultUserAsync();

            var categoryId = await SendAsync(new CreateCategoryCommand
            {
                Title = "Test"
            });
            
            var childCategoryId = await SendAsync(new CreateCategoryCommand()
            {
                Title = "Test Child",
                ParentId = categoryId
            });
            
            var command = new UpdateCategoryCommand()
            {
                Id = childCategoryId,
                Title = "Test Updated",
                ParentCategoryId = null
            };

            await SendAsync(command);

            var childItem =  await FindAsync<Category>(childCategoryId);
            
            childItem.Title.Should().Be("Test Updated");
            childItem.ParentCategoryId.Should().BeNull();
            childItem.LastModifiedBy.Should().NotBeNull();
            childItem.LastModifiedBy.Should().Be(userId);
            childItem.LastModified.Should().NotBeNull();
            childItem.LastModified.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}