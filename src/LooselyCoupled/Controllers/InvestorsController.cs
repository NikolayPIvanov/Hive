using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Investing.Application.Investments.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hive.LooselyCoupled.Controllers
{
    [Authorize]
    public class InvestorsController : ApiControllerBase
    {
        [HttpGet("{id:int}/investments")]
        [Authorize(Roles = "Investor, Admin")]
        public async Task<ActionResult<IEnumerable<InvestmentDto>>> GetInvestments(int id) 
            => Ok(await Mediator.Send(new GetInvestmentsByInvestorQuery(id)));
    }
}