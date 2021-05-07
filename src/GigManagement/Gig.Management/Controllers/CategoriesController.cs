using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Models;
using Hive.Gig.Application.Categories.Commands;
using Hive.Gig.Application.Categories.Queries;
using Hive.Gig.Application.Gigs.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Gig.Management.Controllers
{
    [Authorize]
    public class CategoriesController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, typeof(CategoryDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id, CancellationToken cancellationToken) 
            => await Mediator.Send(new GetCategoryQuery(id), cancellationToken);
        
        [HttpGet]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<CategoryDto>), Description = "Successful operation")]
        public async Task<ActionResult<PaginatedList<CategoryDto>>> GetCategories([FromQuery] GetCategoriesQuery query,
            CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(query, cancellationToken));

        [HttpGet("{id:int}/gigs")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<GigDto>), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<ActionResult<PaginatedList<GigDto>>> GetCategoryGigs([FromRoute] int id,
            [FromQuery] int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
            => Ok(await Mediator.Send(new GetCategoryGigsQuery(id, pageNumber, pageSize), cancellationToken));
        
        [HttpGet("search")]
        [AllowAnonymous]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ICollection<CategoryDto>), Description = "Successful operation")]
        public async Task<ActionResult<ICollection<CategoryDto>>> GetCategoriesByName([FromQuery] GetCategoryByNameQuery query,
            CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(query, cancellationToken));
        
        [HttpPost]
        [SwaggerResponse(HttpStatusCode.Created, typeof(int), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Bad Request operation")]
        public async Task<ActionResult<int>> CreateCategory([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            var id = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetCategoryById), new { id }, id);
        }
        
        [HttpPut("{id:int}")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(void), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Bad Request operation")]
        public async Task<ActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateCategoryCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(void), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<ActionResult> DeleteCategory(int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
            return NoContent();
        }
    }
}