using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Application.GigPackages.Queries.GetGigPackages;
using Hive.Application.Gigs.Commands.CreateGig;
using Hive.Application.Gigs.Commands.DeleteGig;
using Hive.Application.Gigs.Commands.UpdateGig;
using Hive.Application.Gigs.Queries.GetGig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    //[Authorize]
    public class GigsController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GigDto>> Get([FromRoute] int id)
        {
            var entity = await Mediator.Send(new GetGigQuery(id));
            return Ok(entity);
        }
        
        [HttpGet("{id:int}/packages")]
        public async Task<ActionResult<IEnumerable<PackageDto>>> GetPackages([FromRoute] int id)
        {
            var packages = await Mediator.Send(new GetGigPackagesQuery() { GigId = id});
            return Ok(packages);
        }
        
        [HttpPost]
        public async Task<ActionResult<GigDto>> Post([FromBody] CreateGigCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GigDto>> Post(int id, [FromBody] UpdateGigCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GigDto>> Post(int id)
        {
            await Mediator.Send(new DeleteGigCommand() { Id = id });
            return NoContent();
        }
    }
}