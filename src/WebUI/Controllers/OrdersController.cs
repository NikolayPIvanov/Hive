using System;
using System.Threading.Tasks;
using Hive.Application.Orders.Commands.CancelOrder;
using Hive.Application.Orders.Commands.PlaceOrder;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class OrdersController : ApiControllerBase
    {
        // TODO: Return 201
        [HttpPost]
        public async Task<ActionResult<int>> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }
        
        [HttpPut("{number:guid}/cancellation")]
        public async Task<ActionResult<int>> CancelOrder(Guid orderNumber)
        {
            var id = await Mediator.Send(new CancelOrderCommand { OrderNumber = orderNumber });
            return Ok(id);
        }
        
        [HttpPut("{number:guid}/acceptance")]
        public async Task<ActionResult<int>> AcceptOrder(Guid orderNumber)
        {
            var id = await Mediator.Send(new CancelOrderCommand { OrderNumber = orderNumber });
            return Ok(id);
        }
    }
}