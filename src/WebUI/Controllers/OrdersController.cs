using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Hive.Application.Ordering.Orders.Commands;
using Hive.Application.Ordering.Orders.Queries;
using Hive.Application.Ordering.Resolutions.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class OrdersController : ApiControllerBase
    {
        [HttpGet("{orderNumber:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderNumber)
        {
            var order = await Mediator.Send(new GetOrderByOrderNumberQuery(orderNumber));
            return Ok(order);
        }
        
        [HttpGet("personal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderDto>> GetMyOrders()
        {
            var orders = await Mediator.Send(new GetMyOrdersQuery());
            return Ok(orders);
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
        public async Task<ActionResult<int>> CancelOrder(CancelOrderCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/acceptance")]
        public async Task<IActionResult> AcceptOrder([FromBody] AcceptOrderCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/declination")]
        public async Task<IActionResult> DeclineOrder(DeclineOrderCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/progress")]
        public async Task<ActionResult<int>> SetInProgress(SetInProgressOrderCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpPost("{orderNumber:guid}/resolutions")]
        public async Task<ActionResult<Guid>> SubmitResolution([FromRoute] Guid orderNumber, [FromForm] FileUploadForm model)
        {
            var command = new CreateResolutionCommand(orderNumber, model.Version, model.File);
            var resolutionId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetOrder), new { resolutionId }, resolutionId);
        }
        
        [HttpPut("{orderNumber:guid}/resolutions/{resolutionId:int}")]
        public async Task<ActionResult<Guid>> UpdateResolution([FromRoute] Guid orderNumber, int resolutionId, [FromForm] FileUploadForm model)
        {
            await Mediator.Send(new UpdateResolutionCommand(resolutionId, model.Version, model.File));
            return CreatedAtAction(nameof(GetOrder), new { resolutionId }, resolutionId);
        }
    }

    public class FileUploadForm
    {
        public string Version { get; set; }

        public IFormFile File { get; set; }
    }
}