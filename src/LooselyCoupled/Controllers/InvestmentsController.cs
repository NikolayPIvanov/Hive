using System.Threading;
using System.Threading.Tasks;
using Hive.Investing.Application.Investments;
using Hive.Investing.Application.Investments.Commands;
using Hive.Investing.Application.Investments.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hive.LooselyCoupled.Controllers
{
    [Authorize]
    public class InvestmentsController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetInvestment(int id, CancellationToken cancellationToken) =>
            Ok(await Mediator.Send(new GetInvestmentByIdQuery(id), cancellationToken));

        [HttpPost]
        public async Task<IActionResult> MakeInvestment([FromBody] MakeInvestmentCommand command,
            CancellationToken cancellationToken)
        {
            var id = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetInvestment), new {id}, id);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateInvestment([FromRoute] int id, [FromBody] UpdateInvestmentCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.InvestmentId)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInvestment([FromRoute] int id, [FromBody] DeleteInvestmentCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.Id)
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