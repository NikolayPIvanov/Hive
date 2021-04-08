using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Application.GigsManagement.GigPackages.Queries.GetGigPackages;
using Hive.Application.GigsManagement.Gigs.Commands.CreateGig;
using Hive.Application.GigsManagement.Gigs.Commands.DeleteGig;
using Hive.Application.GigsManagement.Gigs.Commands.UpdateGig;
using Hive.Application.GigsManagement.Gigs.Queries;
using Hive.Application.GigsManagement.Gigs.Queries.GetGig;
using Microsoft.AspNetCore.Mvc;
using PackageDto = Hive.Application.GigsManagement.GigPackages.Queries.PackageDto;

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
        
        [HttpPost]
        public async Task<ActionResult<GigDto>> Post([FromBody] CreateGigCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
        
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GigDto>> Update(int id, [FromBody] UpdateGigCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GigDto>> Delete(int id)
        {
            await Mediator.Send(new DeleteGigCommand(id));
            return NoContent();
        }
        
        [HttpGet("{id:int}/packages")]
        public async Task<ActionResult<IEnumerable<PackageDto>>> GetPackages([FromRoute] int id)
        {
            var packages = await Mediator.Send(new GetGigPackagesQuery(id));
            return Ok(packages);
        }
    }
}