using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Security;
using Hive.UserProfile.Application.UserProfiles.Commands;
using Hive.UserProfile.Application.UserProfiles.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Hive.LooselyCoupled.Controllers
{
    [Authorize]
    public class ProfilesController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserProfile(int id, CancellationToken cancellationToken) =>
            Ok(await Mediator.Send(new GetUserProfileByIdQuery(id), cancellationToken));

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetMyUserProfile(string userId, CancellationToken cancellationToken) =>
            Ok(await Mediator.Send(new GetUserProfileByUserIdQuery(userId), cancellationToken));

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] UpdateUserProfileCommand command,
            CancellationToken cancellationToken)
        {
            if (id != command.UserProfileId)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUserProfile(int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteUserProfileCommand(id), cancellationToken);
            return NoContent();
        }
    }
}