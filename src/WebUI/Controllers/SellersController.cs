using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Application.Investing.Plans.Queries;
using Hive.Application.Ordering.Orders.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class SellersController : ApiControllerBase
    {
        [HttpGet("{sellerId}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetSellerOrders(string sellerId)
        {
            return Ok(await Mediator.Send(new GetSellerOrdersQuery(sellerId)));
        } 
        
        [HttpGet("{sellerId:int}/plans")]
        public async Task<ActionResult<IEnumerable<PlanDto>>> GetSellerPlans(int sellerId)
        {
            return Ok(await Mediator.Send(new GetPlansBySellerQuery(sellerId)));
        } 
    }
}