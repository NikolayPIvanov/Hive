using System.Threading.Tasks;
using Hive.Common.Core.Models;
using Hive.Gig.Application.Categories;
using Hive.Gig.Application.Categories.Commands;
using Hive.Gig.Application.Categories.Queries;
using Hive.Gig.Application.Gigs.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hive.LooselyCoupled.Controllers
{
    [Common.Core.Security.Authorize]
    public class CategoriesController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id) => await Mediator.Send(new GetCategoryQuery(id));
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedList<CategoryDto>>> GetCategories([FromQuery] GetCategoriesQuery query) => Ok(await Mediator.Send(query));
        
        
        [HttpGet("{id:int}/gigs")]
        [AllowAnonymous]
        public async Task<ActionResult<PaginatedList<GigDto>>> GetCategoryGigs([FromRoute] int id,
            [FromQuery] int pageNumber = 1, int pageSize = 10)
            => Ok(await Mediator.Send(new GetCategoryGigsQuery(id, pageNumber, pageSize)));
        
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetCategoryById), new { id }, id);
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await Mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}