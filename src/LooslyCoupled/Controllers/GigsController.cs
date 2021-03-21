using System.Threading.Tasks;
using Hive.Gig.Application.Gigs.Commands;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Application.GigScopes.Command;
using Microsoft.AspNetCore.Mvc;
using UpdateGigCommand = Hive.Gig.Application.Gigs.Commands.UpdateGigCommand;

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
        
        [HttpPost("{id}/scope")]
        public async Task<ActionResult<int>> CreateScope(int id, [FromBody] CreateGigScopeCommand command)
        {
            if (id != command.GigId)
            {
                return BadRequest();
            }

            var scopeId = await Mediator.Send(command);
            return Ok(scopeId);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<int>> Update([FromRoute] int id, [FromBody] UpdateGigCommand command)
        { 
            if (id != command.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpPut("{id}/scope")]
        public async Task<IActionResult> UpdateDescription(int id, [FromBody] UpdateGigScopeCommand command)
        {
            if (id != command.GigId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            await Mediator.Send(new DeleteGigCommand(id));
            return NoContent();
        }
    }
}