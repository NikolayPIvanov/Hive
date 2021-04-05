using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Commands;
using Ordering.Application.Orders.Queries;
using Ordering.Application.Resolutions.Queries;
using Ordering.Contracts;

namespace Hive.LooselyCoupled.Controllers
{
    public class OrdersController : ApiControllerBase
    {
        [HttpGet("{orderNumber}")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderNumber)
        {
            var order =  await Mediator.Send(new GetOrderByOrderNumberQuery(orderNumber));
            return Ok(order);
        }
        
        [HttpGet("personal")]
        public async Task<ActionResult<Guid>> GetMyOrders()
        {
            var order =  await Mediator.Send(new GetMyOrdersQuery());
            return Ok(order);
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var orderNumber = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetOrder), new {orderNumber}, orderNumber);
        }
        
        [HttpPut("{orderNumber}/cancellation")]
        public async Task<IActionResult> CancelOrder(Guid orderNumber, [FromBody] CancelOrderCommand command)
        {
            if (orderNumber != command.OrderNumber)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            
            return NoContent();
        }
        
        [HttpPut("{orderNumber}/acceptance")]
        public async Task<IActionResult> AcceptOrder(Guid orderNumber, [FromBody] AcceptOrderCommand command)
        {
            if (orderNumber != command.OrderNumber)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            
            return NoContent();
        }
        
        [HttpPut("{orderNumber}/declination")]
        public async Task<IActionResult> DeclineOrder(Guid orderNumber, [FromBody] DeclineOrderCommand command)
        {
            if (orderNumber != command.OrderNumber)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            
            return NoContent();
        }
        
        [HttpPut("{orderNumber}/progress")]
        public async Task<IActionResult> MarkOrderInProgress(Guid orderNumber, [FromBody] SetInProgressOrderCommand command)
        {
            if (orderNumber != command.OrderNumber)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            
            return NoContent();
        }

        [HttpGet("{orderNumber}/resolutions")]
        public async Task<IActionResult> GetOrderResolutions(Guid orderNumber, [FromQuery] int pageNumber, int pageSize)
        {
            var resolutions = await Mediator.Send(new GetOrderResolutionsQuery(orderNumber, pageNumber, pageSize));
            return Ok(resolutions);
        }
        
        [HttpPost("{orderNumber}/resolutions")]
        public async Task<IActionResult> CreateResolution([FromBody] int x)
        {
            // var resolutions = await Mediator.Send(new GetOrderResolutionsQuery(orderNumber, pageNumber, pageSize));
            // return Ok(resolutions);
            return Ok();
        }
    }
}