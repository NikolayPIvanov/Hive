using System;
using System.IO;
using System.Threading.Tasks;
using Hive.Common.Core.Interfaces;
using Hive.UserProfile.Application.UserProfiles.Commands;
using Hive.UserProfile.Application.UserProfiles.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserProfile.Management.Controllers
{
    public record FileUploadForm(string Version, IFormFile File);

    [Produces("application/json")]
    public class ProfileController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<UserProfileDto>> GetProfile() => Ok(await Mediator.Send(new GetUserProfileByIdQuery()));
        
        [HttpPut("{id:int}/avatar")]
        public async Task<IActionResult> ChangeAvatar(int id, [FromBody] FileUploadForm model)
        {
            var extension = Path.GetExtension(model.File.FileName);
            var command = new UpdateUserAvatarCommand(id, extension, model.File.OpenReadStream());
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] UpdateUserProfileCommand command)
        {
            if (id != command.UserProfileId)
                return BadRequest();
            
            await Mediator.Send(command);
            return NoContent();
        }
    }
}