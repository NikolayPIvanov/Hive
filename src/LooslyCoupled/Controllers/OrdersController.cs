using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Commands;

namespace LooslyCoupled.Controllers
{
    public class OrdersController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Guid>> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var orderNumber = await Mediator.Send(command);
            return Ok(orderNumber);
        }
    }
}