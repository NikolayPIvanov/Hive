using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Hive.Common.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using Ordering.Application.Orders.Commands;
using Ordering.Application.Orders.Queries;
using Ordering.Application.Resolutions.Commands;
using Ordering.Application.Resolutions.Queries;

namespace Ordering.Management.Controllers
{
    [Authorize(Roles = "Seller, Buyer")]
    public class OrdersController : ApiControllerBase
    {
        [HttpGet("{orderNumber:guid}")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderNumber) =>
            Ok(await Mediator.Send(new GetOrderByOrderNumberQuery(orderNumber)));

        [Authorize(Roles = "Buyer, Seller")]
        [HttpGet("my")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<OrderDto>), Description = "Successful operation")]
        public async Task<ActionResult<PaginatedList<OrderDto>>> GetMyOrders([FromQuery] int pageNumber = 1, int pageSize = 20, bool isSeller = true) =>
            Ok(await Mediator.Send(new GetMyOrdersQuery(pageNumber, pageSize, isSeller)));
        
        [HttpPost]
        [Authorize(Roles = "Buyer")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerResponse(HttpStatusCode.Created, typeof(ActionResult<Guid>), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Bad Request operation")]
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
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(ActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Bad Request operation")]
        public async Task<ActionResult> ReviewOrder([FromRoute] Guid orderNumber, [FromBody] ReviewOrderCommand command)
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
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(ActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Bad Request operation")]
        public async Task<ActionResult> SetInProgress([FromRoute] Guid orderNumber, [FromBody] SetInProgressOrderCommand command)
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
        
        [HttpGet("{orderNumber:guid}/resolutions/{version:guid}/file")]
        public async Task<IActionResult> DownloadResolutionFile([FromRoute] Guid version)
        {
            var fileResult = await Mediator.Send(new DownloadResolutionFileQuery(version));
            return fileResult;
        }

        // [HttpGet("{orderNumber:guid}/resolutions")]
        // public async Task<IActionResult> GetResolutions([FromRoute] Guid orderNumber) =>
        //     Ok(await Mediator.Send(new GetResolutionsQuery(orderNumber)));
        
        [HttpPost("{orderNumber:guid}/resolutions")]
        [Authorize(Roles = "Seller")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<Guid>> SubmitResolution([FromRoute] Guid orderNumber, [FromForm] IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var version = Guid.NewGuid().ToString();
            var command = new CreateResolutionCommand(orderNumber, version, extension, file.OpenReadStream());
            var resolutionId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetResolution), new { orderNumber, resolutionId }, 
                new { orderNumber = orderNumber.ToString(), resolutionId });
        }
        
        [HttpPut("{gigId:guid}/resolutions/{version:guid}")]
        [Authorize(Roles = "Buyer")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(ActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Bad Request operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<ActionResult> AcceptResolution([FromRoute] Guid version)
        {
            await Mediator.Send(new AcceptResolutionCommand(version));
            return NoContent();
        }
    }
}