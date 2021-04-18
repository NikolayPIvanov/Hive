using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Hive.Application.Ordering.Orders.Commands;
using Hive.Application.Ordering.Orders.Queries;
using Hive.Application.Ordering.Resolutions.Commands;
using Hive.Application.Ordering.Resolutions.Queries;
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
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyOrders()
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
        public async Task<IActionResult> CancelOrder([FromRoute] Guid orderNumber)
        {
            await Mediator.Send(new CancelOrderCommand(orderNumber));
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/acceptance")]
        public async Task<IActionResult> AcceptOrder([FromRoute] Guid orderNumber, [FromBody] AcceptOrderCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/declination")]
        public async Task<IActionResult> DeclineOrder([FromRoute] Guid orderNumber, [FromBody] DeclineOrderCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/progress")]
        public async Task<ActionResult<int>> SetInProgress([FromRoute] Guid orderNumber, [FromBody] SetInProgressOrderCommand command)
        {
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
        public async Task<ActionResult<Guid>> SubmitResolution([FromRoute] Guid orderNumber, [FromForm] FileUploadForm model)
        {
            var command = new CreateResolutionCommand(orderNumber, model.Version, model.File);
            var resolutionId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetResolution), new { orderNumber, resolutionId }, new { orderNumber = orderNumber.ToString(), resolutionId });
        }
        
        [HttpPut("{orderNumber:guid}/resolutions/{resolutionId:int}")]
        public async Task<ActionResult<Guid>> UpdateResolution([FromRoute] Guid orderNumber, int resolutionId, [FromForm] FileUploadForm model)
        {
            await Mediator.Send(new UpdateResolutionCommand(resolutionId, model.Version, model.File));
            return NoContent();
        }
        
        [HttpPut("{orderNumber:guid}/resolutions/{resolutionId:int}/acceptance")]
        public async Task<ActionResult<Guid>> AcceptResolution([FromRoute] Guid orderNumber, int resolutionId)
        {
            var resolution = await Mediator.Send(new GetResolutionByIdQuery(resolutionId));
            if (resolution.OrderNumber != orderNumber)
            {
                return BadRequest();
            }
            await Mediator.Send(new UpdateResolutionCommand(resolutionId, resolution.Version, null, true));
            await Mediator.Send(new CompleteOrderCommand(orderNumber));
            return NoContent();
        }
    }

    public class FileUploadForm
    {
        public string Version { get; set; }

        public IFormFile File { get; set; }
    }
}