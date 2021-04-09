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
        public async Task<ActionResult<int>> CancelOrder(CancelOrderCommand cancelOrderCommand)
        {
            await Mediator.Send(cancelOrderCommand);
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/acceptance")]
        public async Task<ActionResult<int>> AcceptOrder(AcceptOrderCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }
        
        [HttpPost("{orderNumber:guid}/resolutions")]
        public async Task<ActionResult<Guid>> SubmitResolution([FromRoute] Guid orderNumber, [FromForm] FileUploadForm model)
        {
            var command = new CreateResolutionCommand(orderNumber, model.Version, model.File);
            var resolutionId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetOrder), new { resolutionId }, resolutionId);
        }
    }

    public class FileUploadForm
    {
        public string Version { get; set; }

        public IFormFile File { get; set; }
    }
}