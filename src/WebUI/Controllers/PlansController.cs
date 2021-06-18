using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Investing.Plans.Commands;
using Hive.Application.Investing.Plans.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class PlansController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlan(int id, CancellationToken cancellationToken = default)
        {
            var plan = await Mediator.Send(new GetPlanByIdQuery(id), cancellationToken);
            return Ok(plan);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlanCommand command,
            CancellationToken cancellationToken)
        {
            var id = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetPlan), new {id}, id);
        }
        
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePlan([FromRoute] int id, [FromBody] UpdatePlanCommand command,
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
        public async Task<IActionResult> DeletePlan([FromRoute] int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeletePlanCommand(id), cancellationToken);
            return NoContent();
        }
    }
}