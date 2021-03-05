using System.Threading.Tasks;
using Hive.Application.Categories.Commands.CreateCategory;
using Hive.Application.Categories.Queries.GetCategory;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CategoryDto>> Get([FromQuery] GetCategoryQuery query)
        {
            return await Mediator.Send(query);
        }

        // TODO: Might need to change this to return a dto
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Post([FromBody] CreateCategoryCommand command)
        {
            var id = await Mediator.Send(command);
            var query = new GetCategoryQuery {Id = id};
            return CreatedAtAction(nameof(Get), new { query }, query);
        }
    }
}