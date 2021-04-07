using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Application.Ordering.Orders.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class SellersController : ApiControllerBase
    {
        [HttpGet("{sellerId:int}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetSellerOrders(string sellerId)
        {
            return Ok(await Mediator.Send(new GetSellerOrdersQuery(sellerId)));
        } 
    }
}