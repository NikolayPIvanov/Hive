using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Common.Core.Security;
using Hive.Investing.Application.Investments.Queries;
using Hive.Investing.Application.Investors.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Hive.LooselyCoupled.Controllers
{
    [Authorize]
    public class InvestorsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetMyInvestorId() => Ok(await Mediator.Send(new GetInvestorIdQuery()));
            
        [HttpGet("{id:int}/investments")]
        public async Task<ActionResult<IEnumerable<InvestmentDto>>> GetInvestments(int id) 
            => Ok(await Mediator.Send(new GetInvestmentsQuery(id)));
    }
}