using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders.Commands;
using Ordering.Application.Orders.Queries;
using Ordering.Application.Resolutions.Commands;
using Ordering.Application.Resolutions.Queries;

namespace Hive.LooselyCoupled.Controllers
{
    [Authorize(Roles = "Seller, Buyer, Admin")]
    public class OrdersController : ApiControllerBase
    {
        [HttpGet("{orderNumber:guid}")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderNumber) =>
            Ok(await Mediator.Send(new GetOrderByOrderNumberQuery(orderNumber)));

        [Authorize(Roles = "Buyer")]
        [HttpGet("my")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyOrders() =>
            Ok(await Mediator.Send(new GetMyOrdersQuery()));
        
        [HttpPost]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult<Guid>> PlaceOrder([FromBody] PlaceOrderCommand command)
        {
            var orderNumber = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetOrder), new { orderNumber }, orderNumber);
        }
        
        [HttpPut("{orderNumber:guid}/cancellation")]
        [Authorize(Roles = "Buyer")]
        public async Task<IActionResult> CancelOrder([FromRoute] Guid orderNumber)
        {
            await Mediator.Send(new CancelOrderCommand(orderNumber));
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/review")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> ReviewOrder([FromRoute] Guid orderNumber, [FromBody] ReviewOrderCommand command)
        {
            if (orderNumber != command.OrderNumber)
            {
                return BadRequest();
            }
            
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/progress")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult<int>> SetInProgress([FromRoute] Guid orderNumber, [FromBody] SetInProgressOrderCommand command)
        {
            if (orderNumber != command.OrderNumber)
            {
                return BadRequest();
            }
            
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpGet("{orderNumber:guid}/resolutions/{resolutionId:int}")]
        public async Task<IActionResult> GetResolution([FromRoute] Guid orderNumber, int resolutionId) =>
            Ok(await Mediator.Send(new GetResolutionByIdQuery(resolutionId)));
        
        [HttpGet("{orderNumber:guid}/resolutions/{resolutionId:int}/file")]
        public async Task<IActionResult> DownloadResolutionFile([FromRoute] int resolutionId)
        {
            var file = await Mediator.Send(new DownloadResolutionFileQuery(resolutionId));
            return File(file.Source, file.ContentType, file.FileName);
        }

        [HttpGet("{orderNumber:guid}/resolutions")]
        public async Task<IActionResult> GetResolutions([FromRoute] Guid orderNumber) =>
            Ok(await Mediator.Send(new GetResolutionsQuery(orderNumber)));
        
        [HttpPost("{orderNumber:guid}/resolutions")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult<Guid>> SubmitResolution([FromRoute] Guid orderNumber, [FromForm] FileUploadForm model)
        {
            var extension = Path.GetExtension(model.File.FileName);
            var command = new CreateResolutionCommand(orderNumber, model.Version, extension, model.File.OpenReadStream());
            var resolutionId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetResolution), new { orderNumber, resolutionId }, 
                new { orderNumber = orderNumber.ToString(), resolutionId });
        }
        
        [HttpPut("{orderNumber:guid}/resolutions/{resolutionId:int}/acceptance")]
        [Authorize(Roles = "Buyer")]
        public async Task<ActionResult<Guid>> AcceptResolution([FromRoute] Guid orderNumber, int resolutionId)
        {
            await Mediator.Send(new AcceptResolutionCommand(resolutionId));
            return NoContent();
        }
    }

    public record FileUploadForm(string Version, IFormFile File);
}