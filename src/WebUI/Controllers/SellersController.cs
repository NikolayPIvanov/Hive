using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Application.Orders.Queries.GetSellerOrders;
using Hive.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class SellersController : ApiControllerBase
    {
        [HttpGet("{sellerId:int}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetSellerOrders(int sellerId, [FromQuery] OrderStatus? orderStatus)
        {
            return Ok(await Mediator.Send(new GetSellerOrdersQuery.Query(sellerId, orderStatus)));
        } 
    }
}