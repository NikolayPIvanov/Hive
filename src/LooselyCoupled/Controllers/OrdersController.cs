using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Commands;
using Ordering.Application.Orders.Queries;

namespace Hive.LooselyCoupled.Controllers
{
    public class OrdersController : ApiControllerBase
    {
        [HttpGet("{orderNumber}")]
        public async Task<ActionResult<Guid>> GetOrder(Guid orderNumber)
        {
            var order =  await Mediator.Send(new GetOrderByOrderNumberQuery(orderNumber));
            return Ok(order);
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var orderNumber = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetOrder), new {orderNumber}, orderNumber);
        }
    }
}