using System.Threading;
using System.Threading.Tasks;
using Hive.Application.UserProfiles.Commands.UpdateUserProfile;
using Hive.Application.UserProfiles.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    [Authorize]
    public class ProfilesController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserProfile(int id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new GetUserProfileByIdQuery(id), cancellationToken));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] UpdateProfileCommand.Command command, CancellationToken cancellationToken)
        {
            if (id != command.ProfileId)
            {
                return BadRequest();
            }
            
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}