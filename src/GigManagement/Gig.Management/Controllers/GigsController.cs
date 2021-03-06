﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Models;
using Hive.Gig.Application.GigPackages;
using Hive.Gig.Application.GigPackages.Commands;
using Hive.Gig.Application.GigPackages.Queries;
using Hive.Gig.Application.Gigs.Commands;
using Hive.Gig.Application.Gigs.Commands.CreateGig;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Application.Reviews.Commands;
using Hive.Gig.Application.Reviews.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Gig.Management.Controllers
{
    public record FileUpload(string FileData);
    
    [Produces("application/json")]
    [Authorize]
    public class GigsController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(GigDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Anomaly not found")]
        public async Task<ActionResult<GigDto>> GetGigById([FromRoute] int id, CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(new GetGigQuery(id), cancellationToken));
        
        [HttpGet("packages/{packageId:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(GigDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Anomaly not found")]
        public async Task<ActionResult<GigDto>> GetGigByPackageId([FromRoute] int packageId, CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(new GetGigByPackageIdQuery(packageId), cancellationToken));
            
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<GigDto>), Description = "Successful operation")]
        public async Task<ActionResult<PaginatedList<GigOverviewDto>>> GetGigs([FromQuery] GetGigsQuery request, CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(new GetCategoryGigsQuery(null, request), cancellationToken));

        [HttpGet("random")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<GigOverviewDto>), Description = "Successful operation")]
        public async Task<ActionResult<PaginatedList<GigOverviewDto>>> GetRandom([FromQuery] GetRandomGigsQuery request, CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(request, cancellationToken));

        [HttpPost]
        [Authorize(Roles = "Seller, Admin")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(int), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Bad Request operation")]
        public async Task<ActionResult<int>> CreateGig([FromBody] CreateGigCommand command, CancellationToken cancellationToken)
        {
            var id = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetGigById), new {id}, id);
        }
        
        [HttpPut("{id:int}/images")]
        [Authorize(Roles = "Seller, Admin")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Bad Request operation")]
        public async Task<IActionResult> UpdateImage([FromRoute] int id, [FromBody] FileUpload file, CancellationToken cancellationToken)
        {
            var extension = ".png";
            var imageDataByteArray = Convert.FromBase64String(file.FileData);
            var imageDataStream = new MemoryStream(imageDataByteArray) {Position = 0};

            var command = new SetGigImageCommand(id, extension, imageDataStream);
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
        
        [HttpGet("{id:int}/images")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [SwaggerResponse(200, typeof(FileContentResult))]
        public async Task<FileContentResult> GetAvatar([FromRoute] int id)
        {
            return await Mediator.Send(new GetGigImageQuery(id));
        }
       
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Seller, Admin")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Invalid ID supplied")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Anomaly not found")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGigCommand command, CancellationToken cancellationToken)
        { 
            if (id != command.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }

        // TODO: Cannot delete if there are orders in process or if there is a plan active
        [HttpDelete("{id}")]
        [Authorize(Roles = "Seller, Admin")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ProblemDetails), Description = "Invalid ID supplied")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteGigCommand(id), cancellationToken);
            return NoContent();
        }
        
        [HttpGet("{id:int}/packages/{packageId:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PackageDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<ActionResult<PackageDto>> GetPackageById(int id, int packageId, CancellationToken cancellationToken) =>
            Ok(await Mediator.Send(new GetPackageQuery(packageId), cancellationToken));
        
        [HttpGet("{id:int}/packages")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<PackageDto>), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<ActionResult<IEnumerable<PackageDto>>> GetPackages([FromRoute] int id, CancellationToken cancellationToken)
            => Ok(await Mediator.Send(new GetGigPackagesQuery(id), cancellationToken));
        
        // [HttpPost("{id:int}/packages")]
        // [Authorize(Roles = "Seller, Admin")]
        // [SwaggerResponse(HttpStatusCode.Created, typeof(int), Description = "Successful operation")]
        // [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ProblemDetails), Description = "Invalid ID supplied")]
        // [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        // public async Task<ActionResult<int>> CreatePackage([FromRoute] int id, [FromBody] CreatePackageCommand command,
        //     CancellationToken cancellationToken)
        // {
        //     var packageId = await Mediator.Send(command, cancellationToken);
        //     return CreatedAtAction(nameof(GetPackageById), new {id, packageId}, new {id, packageId});
        // }
        //
        
        [HttpPut("{id:int}/packages/{packageId:int}")]
        [Authorize(Roles = "Seller, Admin")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ProblemDetails), Description = "Invalid ID supplied")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<IActionResult> UpdatePackage([FromRoute] int id, int packageId,
            [FromBody] UpdatePackageCommand command, CancellationToken cancellationToken)
        {
            if (packageId != command.PackageId || id != command.GigId)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id:int}/packages/{packageId:int}")]
        [Authorize(Roles = "Seller, Admin")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<IActionResult> DeletePackage([FromRoute] int id, int packageId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeletePackageCommand(packageId), cancellationToken);
            return NoContent();
        }

        [HttpGet("{gigId:int}/reviews/{reviewId:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(ReviewDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<ActionResult<ReviewDto>> GetReviewById([FromRoute] int gigId, int reviewId, CancellationToken cancellationToken) =>
            Ok(await Mediator.Send(new GetReviewByIdQuery(gigId, reviewId), cancellationToken));
        
        [HttpGet("{gigId:int}/reviews")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<ReviewDto>), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<ActionResult<PaginatedList<ReviewDto>>> GetReviewsList(
            [FromRoute] int gigId, [FromQuery] int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default) =>
            Ok(await Mediator.Send(new GetGigReviewsQuery(gigId, pageNumber, pageSize), cancellationToken));

        [HttpPost("{gigId:int}/reviews")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(int), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ProblemDetails), Description = "Invalid ID supplied")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<IActionResult> CreateReview([FromRoute] int gigId, [FromBody] CreateReviewCommand command, 
            CancellationToken cancellationToken)
        {
            if (command.GigId != gigId)
            {
                return BadRequest();
            }

            var reviewId = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetReviewById), new {gigId, reviewId}, new {gigId, reviewId});
        }

        [HttpPut("{gigId:int}/reviews/{reviewId:int}")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ProblemDetails), Description = "Invalid ID supplied")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<IActionResult> UpdateReview([FromRoute] int gigId, int reviewId,
            [FromBody] UpdateReviewCommand command, CancellationToken cancellationToken)
        {
            if (command.Id != reviewId)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{gigId:int}/reviews/{reviewId:int}")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<IActionResult> DeleteReview([FromRoute] int gigId, int reviewId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteReviewCommand(reviewId), cancellationToken);
            return NoContent();
        }
    }
}