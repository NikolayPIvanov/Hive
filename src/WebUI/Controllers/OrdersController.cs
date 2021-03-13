using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Hive.Application.Orders.Commands.AcceptOrder;
using Hive.Application.Orders.Commands.CancelOrder;
using Hive.Application.Orders.Commands.PlaceOrder;
using Hive.Application.Orders.Queries.GetSellerOrders;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class OrdersController : ApiControllerBase
    {
        [HttpGet("{orderNumber:guid}")]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderNumber)
        {
            return Ok();
        }
        
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Guid>> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var orderNumber = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetOrder), new { orderNumber }, orderNumber);
        }
        
        [HttpPut("{orderNumber:guid}/cancellation")]
        public async Task<ActionResult<int>> CancelOrder(Guid orderNumber)
        {
            await Mediator.Send(new CancelOrderCommand { OrderNumber = orderNumber });
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/acceptance")]
        public async Task<ActionResult<int>> AcceptOrder(Guid orderNumber)
        {
            var id = await Mediator.Send(new AcceptOrderCommand() { OrderNumber = orderNumber });
            return Ok(id);
        }
    }
}