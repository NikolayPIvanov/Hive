using System.Threading.Tasks;
using Hive.Application.Gigs.Commands.CreateGig;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class GigsController : ApiControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<int>> Post([FromBody] CreateGigCommand command)
        {
            var id = await Mediator.Send(command);
            return Ok(id);
        }
    }
}