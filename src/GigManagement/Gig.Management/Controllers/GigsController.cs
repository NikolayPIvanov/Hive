using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Models;
using Hive.Gig.Application.GigPackages;
using Hive.Gig.Application.GigPackages.Commands;
using Hive.Gig.Application.GigPackages.Queries;
using Hive.Gig.Application.Gigs.Commands;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Application.Reviews.Commands;
using Hive.Gig.Application.Reviews.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gig.Management.Controllers
{
    public class GigsController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GigDto>> GetGigById([FromRoute] int id, CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(new GetGigQuery(id), cancellationToken));
        
        [HttpPost]
        [Authorize(Roles = "Seller")]
        // TODO: Raise event if the gig is related to a plan so investors can be notified.
        public async Task<ActionResult<int>> Post([FromBody] CreateGigCommand command, CancellationToken cancellationToken)
        {
            var id = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetGigById), new {id}, id);
        }
       
        [HttpPut("{id}")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGigCommand command, CancellationToken cancellationToken)
        { 
            if (id != command.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Seller")]
        // TODO: Cannot delete if there are orders in process or if there is a plan active
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteGigCommand(id), cancellationToken);
            return NoContent();
        }
        
        [HttpGet("{id:int}/packages/{packageId:int}")]
        public async Task<ActionResult<PackageDto>> GetPackageById(int id, int packageId, CancellationToken cancellationToken) =>
            Ok(await Mediator.Send(new GetPackageQuery(packageId), cancellationToken));
        
        [HttpGet("{id:int}/packages")]
        public async Task<ActionResult<IEnumerable<PackageDto>>> GetPackages([FromRoute] int id, CancellationToken cancellationToken)
            => Ok(await Mediator.Send(new GetGigPackagesQuery(id), cancellationToken));
        
        [HttpPost("{id:int}/packages")]
        [Authorize(Roles = "Seller")]
        public async Task<ActionResult<int>> CreatePackage([FromRoute] int id, [FromBody] CreatePackageCommand command,
            CancellationToken cancellationToken)
        {
            if (command.GigId != id)
            {
                return BadRequest();
            }
            
            var packageId = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetPackageById), new {id, packageId}, new {id, packageId});
        }
        
        [HttpPut("{id:int}/packages/{packageId:int}")]
        [Authorize(Roles = "Seller")]
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
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> DeletePackage([FromRoute] int id, int packageId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeletePackageCommand(packageId), cancellationToken);
            return NoContent();
        }

        [HttpGet("{gigId:int}/reviews/{reviewId:int}")]
        public async Task<ActionResult<ReviewDto>> GetReviewById([FromRoute] int gigId, int reviewId, CancellationToken cancellationToken) =>
            Ok(await Mediator.Send(new GetReviewByIdQuery(gigId, reviewId), cancellationToken));
        
        [HttpGet("{gigId:int}/reviews")]
        public async Task<ActionResult<PaginatedList<ReviewDto>>> GetReviewsList(
            [FromRoute] int gigId, [FromQuery] int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default) =>
            Ok(await Mediator.Send(new GetGigReviewsQuery(gigId, pageNumber, pageSize), cancellationToken));

        [HttpPost("{gigId:int}/reviews")]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> DeleteReview([FromRoute] int gigId, int reviewId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteReviewCommand(reviewId), cancellationToken);
            return NoContent();
        }
    }
}