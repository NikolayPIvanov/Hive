using System.Threading.Tasks;
using Hive.Application.Common.Security;
using Hive.Application.Gigs.Commands.CreateGig;
using Hive.Application.Gigs.Queries.GetGig;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    [Authorize]
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
    }
}