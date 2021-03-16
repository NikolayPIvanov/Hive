using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Hive.Application.Categories.Commands.CreateCategory;
using Hive.Application.Categories.Commands.DeleteCategory;
using Hive.Application.Common.Exceptions;
using Hive.Application.TodoItems.Commands.CreateTodoItem;
using Hive.Application.TodoItems.Commands.DeleteTodoItem;
using Hive.Application.TodoLists.Commands.CreateTodoList;
using Hive.Domain.Entities;
using Hive.Domain.Entities.Categories;
using NUnit.Framework;

namespace Hive.Application.IntegrationTests.Categories.Commands
{
    using static Testing;
    
    public class DeleteCategoryTests : TestBase
    {
        [Test]
        public void ShouldRequireValidCategoryId()
        {
            var command = new DeleteCategoryCommand { Id = 99 };

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteCategory()
        {
            const int childCategoriesCount = 3;
            var categoryId = await SendAsync(new CreateCategoryCommand
            {
                Title = "New Category"
            });
        
            await AddBulkCategories(childCategoriesCount, categoryId);

            await SendAsync(new DeleteCategoryCommand
            {
                Id = categoryId
            });

            var category = await FindAsync<TodoItem>(categoryId);
            var categoriesCount = await CountAsync<Category>();

            category.Should().BeNull();
            categoriesCount.Should().Be(childCategoriesCount);
        }

        private async Task AddBulkCategories(int count, int parentId)
        {
            var categories = Enumerable.Range(1, count).Select(
                _ => new Category
                {
                    Title = Guid.NewGuid().ToString(),
                    ParentCategoryId = parentId
                });
            await AddBulkAsync<Category>(categories);
        }
    }
}