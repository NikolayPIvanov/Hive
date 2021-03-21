using System.Threading.Tasks;
using Hive.Gig.Application.Gigs.Commands;
using Microsoft.AspNetCore.Mvc;

namespace LooslyCoupled.Controllers
{
    public class GigsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateGigCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }
    }
}