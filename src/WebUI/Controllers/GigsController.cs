using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Application.Common.Models;
using Hive.Application.GigsManagement.GigPackages.Commands.CreatePackage;
using Hive.Application.GigsManagement.GigPackages.Commands.DeletePackage;
using Hive.Application.GigsManagement.GigPackages.Commands.UpdatePackage;
using Hive.Application.GigsManagement.GigPackages.Queries;
using Hive.Application.GigsManagement.GigPackages.Queries.GetGigPackages;
using Hive.Application.GigsManagement.GigPackages.Queries.GetPackage;
using Hive.Application.GigsManagement.Gigs.Commands.CreateGig;
using Hive.Application.GigsManagement.Gigs.Commands.DeleteGig;
using Hive.Application.GigsManagement.Gigs.Commands.UpdateGig;
using Hive.Application.GigsManagement.Gigs.Queries;
using Hive.Application.GigsManagement.Gigs.Queries.GetGig;
using Hive.Application.GigsManagement.Gigs.Queries.GetMyGigs;
using Hive.Application.GigsManagement.Reviews.Commands;
using Hive.Application.GigsManagement.Reviews.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class GigsController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GigDto>> Get([FromRoute] int id) => Ok(await Mediator.Send(new GetGigQuery(id)));

        [HttpGet("personal")]
        public async Task<ActionResult<IEnumerable<GigDto>>> GetPackages() =>
            Ok(await Mediator.Send(new GetMyGigsQuery()));

        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateGigCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {id}, id);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<GigDto>> Update(int id, [FromBody] UpdateGigCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<GigDto>> Delete(int id)
        {
            await Mediator.Send(new DeleteGigCommand(id));
            return NoContent();
        }

        [HttpGet("{id:int}/packages/{packageId:int}")]
        public async Task<ActionResult<PackageDto>> GetPackageById(int id, int packageId)
        {
            var entity = await Mediator.Send(new GetPackageQuery(packageId));
            return Ok(entity);
        }

        [HttpGet("{id:int}/packages")]
        public async Task<ActionResult<IEnumerable<PackageDto>>> GetPackages([FromRoute] int id)
        {
            var packages = await Mediator.Send(new GetGigPackagesQuery(id));
            return Ok(packages);
        }

        [HttpPost("{id:int}/packages")]
        public async Task<ActionResult<IEnumerable<PackageDto>>> CreatePackage([FromBody] CreatePackageCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {id}, id);
        }

        [HttpPut("{id:int}/packages/{packageId:int}")]
        public async Task<IActionResult> UpdatePackage([FromRoute] int id, int packageId,
            [FromBody] UpdatePackageCommand command)
        {
            if (packageId != command.PackageId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id:int}/packages/{packageId:int}")]
        public async Task<IActionResult> UpdatePackage([FromRoute] int id, int packageId)
        {
            await Mediator.Send(new DeletePackageCommand(packageId));
            return NoContent();
        }

        [HttpGet("{gigId:int}/reviews/{reviewId:int}")]
        public async Task<ActionResult<ReviewDto>> GetReviewById([FromRoute] int gigId, int reviewId) =>
            Ok(await Mediator.Send(new GetReviewByIdQuery(gigId, reviewId)));
        
        [HttpGet("{gigId:int}/reviews")]
        public async Task<ActionResult<PaginatedList<ReviewDto>>> GetReviewsList([FromRoute] int gigId, [FromQuery] int pageNumber = 1, int pageSize = 10) =>
            Ok(await Mediator.Send(new GetGigReviewsQuery(gigId, pageNumber, pageSize)));

        [HttpPost("{gigId:int}/reviews")]
        public async Task<IActionResult> CreateReview([FromRoute] int gigId, [FromBody] CreateReviewCommand command)
        {
            if (command.GigId != gigId)
            {
                return BadRequest();
            }

            var reviewId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetReviewById), new {gigId, reviewId}, new {gigId, reviewId});
        }

        [HttpPut("{gigId:int}/reviews/{reviewId:int}")]
        public async Task<IActionResult> UpdateReview([FromRoute] int gigId, int reviewId,
            [FromBody] UpdateReviewCommand command)
        {
            if (command.Id != reviewId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{gigId:int}/reviews/{reviewId:int}")]
        public async Task<IActionResult> DeleteReview([FromRoute] int gigId, int reviewId)
        {
            await Mediator.Send(new DeleteReviewCommand(reviewId));
            return NoContent();
        }

    }
}