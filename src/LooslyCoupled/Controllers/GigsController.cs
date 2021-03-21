using System.Threading.Tasks;
using Hive.Gig.Application.Gigs.Commands;
using Hive.Gig.Application.Gigs.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LooslyCoupled.Controllers
{
    public class GigsController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<int>> Get([FromRoute] int id)
        {
            var gig = await Mediator.Send(new GetGigQuery(id));
            return Ok(gig);
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateGigCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
    }
}