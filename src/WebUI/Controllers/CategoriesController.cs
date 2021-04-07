using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Application.Categories.Commands.CreateCategory;
using Hive.Application.Categories.Commands.DeleteCategory;
using Hive.Application.Categories.Commands.UpdateCategory;
using Hive.Application.Categories.Queries.GetCategories;
using Hive.Application.Categories.Queries.GetCategory;
using Hive.Application.Common.Models;
using Hive.Application.Gigs.Queries.GetCategoryGigs;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    //[Authorize]
    public class CategoriesController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            return await Mediator.Send(new GetCategoryQuery(id));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get([FromQuery] GetCategoriesQuery query)
        {
            return Ok(await Mediator.Send(query));
        }
        
        [HttpGet("{id:int}/gigs")]
        public async Task<ActionResult<PaginatedList<CategoryDto>>> GetGigs([FromRoute] int id, [FromQuery] int? pageNumber, [FromQuery] int? pageSize)
        {
            var query = new GetCategoryGigsQuery(id);
            if(pageNumber.HasValue)
            {
                query = query with {PageNumber = pageNumber.Value};
            }

            if (pageSize.HasValue)
            {
                query = query with {PageSize = pageSize.Value};
            }

            var list = await Mediator.Send(query);
            
            return Ok(list);
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
            await Mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}