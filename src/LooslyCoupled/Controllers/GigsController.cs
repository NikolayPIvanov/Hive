using System.Threading.Tasks;
using Gig.Contracts;
using Hive.Gig.Application.Gigs.Commands;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Application.GigScopes.Command;
using Hive.Gig.Application.GigScopes.Queries;
using Microsoft.AspNetCore.Mvc;
using UpdateGigCommand = Hive.Gig.Application.Gigs.Commands.UpdateGigCommand;

namespace LooslyCoupled.Controllers
{
    public class GigsController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<GigDto>> Get([FromRoute] int id)
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
       
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGigCommand command)
        { 
            if (id != command.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteGigCommand(id));
            return NoContent();
        }
        
        [HttpGet("{id}/scopes")]
        public async Task<ActionResult<GigScopeDto>> GetScope([FromRoute] int id)
        {
            await Mediator.Send(new GetGigScopeQuery(id));
            return NoContent();
        }
        
        [HttpPost("{id}/scopes")]
        public async Task<ActionResult<int>> CreateScope(int id, [FromBody] CreateGigScopeCommand command)
        {
            if (id != command.GigId)
            {
                return BadRequest();
            }

            var scopeId = await Mediator.Send(command);
            return Ok(scopeId);
        }
        
        [HttpPut("{id}/scopes")]
        public async Task<IActionResult> UpdateDescription(int id, [FromBody] UpdateGigScopeCommand command)
        {
            if (id != command.GigId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }
        
        // [HttpGet("{id}/packges/{id}")]
        // public async Task<AcceptedResult>
        //
        // [HttpGet("{id}/packges/{id}")]
    }
}