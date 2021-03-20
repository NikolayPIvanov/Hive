using System.Threading.Tasks;
using Hive.Gig.Application.Categories.Commands.CreateCategory;
using Microsoft.AspNetCore.Mvc;

namespace LooslyCoupled.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await Mediator.Send(command);

            return Ok(id);
        }
    }
}