using System.Threading;
using System.Threading.Tasks;
using Hive.Investing.Application.Investments.Commands;
using Hive.Investing.Application.Investments.Queries;
using Hive.Investing.Application.Plans.Commands;
using Hive.Investing.Application.Plans.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hive.LooselyCoupled.Controllers
{
    [Authorize(Roles = "Seller,Investor,Admin")]
    public class PlansController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPlan(int id, CancellationToken cancellationToken = default) 
            => Ok(await Mediator.Send(new GetPlanByIdQuery(id), cancellationToken));

        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> CreatePlan([FromBody] CreatePlanCommand command,
            CancellationToken cancellationToken = default)
        {
            var id = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetPlan), new {id}, id);
        }
        
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Seller")]
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

        [HttpGet("{planId:int}/investments")]
        public async Task<IActionResult> GetPlanInvestments([FromRoute] int planId, CancellationToken cancellationToken)
            => Ok(await Mediator.Send(new GetInvestmentsByPlanQuery(planId), cancellationToken));
        
        [HttpGet("{planId:int}/investments/{investmentId:int}")]
        public async Task<IActionResult> GetInvestment([FromRoute] int planId, int investmentId, CancellationToken cancellationToken)
            => Ok(await Mediator.Send(new GetInvestmentByIdQuery(planId, investmentId), cancellationToken));
        
        [HttpPost]
        [Authorize(Roles = "Investor")]
        public async Task<IActionResult> MakeInvestment([FromBody] MakeInvestmentCommand command,
            CancellationToken cancellationToken)
        {
            var id = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetInvestment), new {id}, id);
        }
        
        [HttpPut("{planId:int}/investments/{investmentId:int}")]
        public async Task<IActionResult> UpdateInvestment([FromRoute] int planId, int investmentId, [FromBody] UpdateInvestmentCommand command,
            CancellationToken cancellationToken)
        {
            // TODO: Should return 404 if planId is not found
            if (investmentId != command.InvestmentId)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
        
        
        [HttpDelete("{planId:int}/investments/{investmentId:int}")]
        public async Task<IActionResult> DeleteInvestment([FromRoute] int planId, int investmentId, [FromBody] DeleteInvestmentCommand command,
            CancellationToken cancellationToken)
        {
            // TODO: Should return 404 if planId is not found
            if (investmentId != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
    
        [HttpPut("{id:int}/processing")]
        public async Task<IActionResult> ProcessInvestment([FromRoute] int id, [FromBody] ProcessInvestmentCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.InvestmentId)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}