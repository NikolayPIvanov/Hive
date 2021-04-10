using System.Threading;
using System.Threading.Tasks;
using Hive.Application.Investing.Investments.Commands;
using Hive.Application.Investing.Investments.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
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
        
        [HttpPut("{id:int}/review")]
        public async Task<IActionResult> AcceptInvestment([FromRoute] int id, [FromBody] ProcessInvestmentCommand command,
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