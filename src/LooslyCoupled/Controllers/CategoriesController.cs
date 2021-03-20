using System.Threading.Tasks;
using Gig.Contracts;
using Hive.Gig.Application.Categories.Commands;
using Hive.Gig.Application.Categories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LooslyCoupled.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get([FromRoute] int id)
        {
            var category = await Mediator.Send(new GetCategoryQuery(id));
            return Ok(category);
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }
    }
}