using System.Threading.Tasks;
using Hive.Application.Categories.Commands.CreateCategory;
using Hive.Application.Categories.Commands.DeleteCategory;
using Hive.Application.Categories.Commands.UpdateCategory;
using Hive.Application.Categories.Queries.GetCategory;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            return await Mediator.Send(new GetCategoryQuery { Id = id});
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Post([FromBody] CreateCategoryCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateCategoryCommand command)
        {
            // TODO: Check command
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteCategoryCommand() { Id = id});
            return NoContent();
        }
    }
}