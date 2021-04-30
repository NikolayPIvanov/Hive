﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Security;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Application.Sellers.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Gig.Management.Controllers
{
    [Authorize]
    public class SellersController : ApiControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUserSellerId(CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(new GetSellerIdQuery(), cancellationToken));
            
        [HttpGet("{id:int}/gigs")]
        public async Task<ActionResult<IEnumerable<GigDto>>> GetMyGigs(CancellationToken cancellationToken) 
            => Ok(await Mediator.Send(new GetMyGigsQuery(), cancellationToken));
    }
}