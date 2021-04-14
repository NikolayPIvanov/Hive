using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Application.Common.Models;
using Hive.Application.GigsManagement.Categories.Commands.CreateCategory;
using Hive.Application.GigsManagement.Categories.Commands.DeleteCategory;
using Hive.Application.GigsManagement.Categories.Commands.UpdateCategory;
using Hive.Application.GigsManagement.Categories.Queries.GetCategories;
using Hive.Application.GigsManagement.Categories.Queries.GetCategory;
using Hive.Application.GigsManagement.Gigs.Queries;
using Hive.Application.GigsManagement.Gigs.Queries.GetCategoryGigs;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoryDto>> Get(int id) => await Mediator.Send(new GetCategoryByIdQuery(id));
        
        [HttpGet]
        public async Task<ActionResult<PaginatedList<CategoryDto>>> Get([FromQuery] GetCategoriesQuery query) => Ok(await Mediator.Send(query));

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
        
        
        [HttpGet("{id:int}/gigs")]
        public async Task<ActionResult<PaginatedList<GigDto>>> GetGigs([FromRoute] int id, [FromQuery] int pageNumber = 1, int pageSize = 10)
        {
            var list = await Mediator.Send(new GetCategoryGigsQuery(id, pageNumber, pageSize));
            return Ok(list);
        }
    }
}