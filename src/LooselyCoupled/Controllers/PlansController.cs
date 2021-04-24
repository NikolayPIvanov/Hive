using System.Threading;
using System.Threading.Tasks;
using Hive.Investing.Application.Plans.Commands;
using Hive.Investing.Application.Plans.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hive.LooselyCoupled.Controllers
{
    [Authorize]
    public class PlansController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlan(int id, CancellationToken cancellationToken = default) 
            => Ok(await Mediator.Send(new GetPlanByIdQuery(id), cancellationToken));

        [HttpPost]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlanCommand command,
            CancellationToken cancellationToken = default)
        {
            var id = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetPlan), new {id}, id);
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePlan([FromRoute] int id, [FromBody] UpdatePlanCommand command,
            CancellationToken cancellationToken = default)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePlan([FromRoute] int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeletePlanCommand(id), cancellationToken);
            return NoContent();
        }
    }
}