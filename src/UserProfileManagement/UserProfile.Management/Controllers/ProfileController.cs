using System;
using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Hive.UserProfile.Application.UserProfiles.Commands;
using Hive.UserProfile.Application.UserProfiles.Queries;
using Microsoft.AspNetCore.Mvc;

namespace UserProfile.Management.Controllers
{
    [Produces("application/json")]
    public class ProfileController : ApiControllerBase
    {
        private readonly ICurrentUserService _currentUserService;

        public ProfileController(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        [HttpGet]
        public async Task<ActionResult<UserProfileDto>> GetProfile() =>
            Ok(await Mediator.Send(new GetUserProfileByIdQuery()));

        [HttpPut("{id:int}")]
        public async Task<ActionResult<UserProfileDto>> UpdateProfile(int id, [FromBody] UpdateUserProfileCommand command)
        {
            if (id != command.UserProfileId)
                return BadRequest();
            
            await Mediator.Send(command);
            return NoContent();
        }
    }
}