using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Models;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Application.Sellers.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Gig.Management.Controllers
{
    [Authorize]
    public class SellersController : ApiControllerBase
    {
        [HttpGet]
        [Consumes("application/json")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(string), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<ActionResult<string>> GetUserSellerId(CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(new GetSellerIdQuery(), cancellationToken));
            
        [HttpGet("{id:int}/gigs")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<GigOverviewDto>), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(ProblemDetails), Description = "Anomaly not found")]
        public async Task<ActionResult<PaginatedList<GigOverviewDto>>> GetMyGigs([FromQuery] GetMyGigsQuery request, CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(request, cancellationToken));
    }
}