using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Queries;
using Ordering.Contracts;

namespace Hive.LooselyCoupled.Controllers
{
    public class SellersController : ApiControllerBase
    {
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderNumber)
        {
            var order =  await Mediator.Send(new GetOrderByOrderNumberQuery(orderNumber));
            return Ok(order);
        }
    }
}