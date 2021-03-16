using System.Threading.Tasks;
using FluentAssertions;
using Hive.Application.Categories.Commands.CreateCategory;
using Hive.Domain.Entities.Categories;
using NUnit.Framework;
using ValidationException = Hive.Application.Common.Exceptions.ValidationException;

namespace Hive.Application.IntegrationTests.Categories.Commands
{
    using static Testing;
    
    public class CreateCategoryTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateCategoryCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }
        
        [Test]
        public async Task ShouldCreateBaseCategory()
        {
            const string title = "Base Category";
            var categoryId = await SendAsync(new CreateCategoryCommand
            {
                Title = title
            });

            var item = await FindAsync<Category>(categoryId);

            item.Should().NotBeNull();
            item.Title.Should().Be(title);
            item.Gigs.Should().BeEmpty();
            item.ParentCategoryId.Should().BeNull();
            item.SubCategories.Should().BeEmpty();
        }
        
        [Test]
        public async Task ShouldCreateCategoryWithSubCategories()
        {
            const string title = "Base Category";
            const string childTitle = "Child";
            var baseCategoryId = await SendAsync(new CreateCategoryCommand
            {
                Title = title
            });
            
            var childCategoryId = await SendAsync(new CreateCategoryCommand
            {
                Title = childTitle,
                ParentId = baseCategoryId
            });
    
            var item = await FindAsync<Category>(childCategoryId);

            item.Should().NotBeNull();
            item.Title.Should().Be(childTitle);
            item.Gigs.Should().BeEmpty();
            item.ParentCategoryId.Should().Be(baseCategoryId);
        }
    }
}