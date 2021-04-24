using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Application.Sellers.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Hive.LooselyCoupled.Controllers
{
    [Authorize]
    public class SellersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetMySellerId() => Ok(await Mediator.Send(new GetSellerIdQuery()));
            
        [HttpGet("{id:int}/gigs")]
        public async Task<ActionResult<IEnumerable<GigDto>>> GetMyGigs() => Ok(await Mediator.Send(new GetMyGigsQuery()));
    }
}