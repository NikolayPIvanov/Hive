using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Hive.Common.Core.Security;
using Hive.UserProfile.Application.UserProfiles.Commands;
using Hive.UserProfile.Application.UserProfiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace UserProfile.Management.Controllers
{
    public record FileUpload(string FileData);
    
    [Authorize]
    public class ProfileController : ApiControllerBase
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(UserProfileDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(NotFoundObjectResult))]
        public async Task<ActionResult<UserProfileDto>> GetProfile() => Ok(await Mediator.Send(new GetUserProfileByIdQuery()));

        [HttpPut("{id:int}/names")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerResponse(StatusCodes.Status204NoContent, typeof(Unit))]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(NotFoundObjectResult))]
        public async Task<ActionResult<Unit>> UpdateProfileNames([FromRoute] int id,
            [FromBody] UpdateUserNamesCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpPut("{id:int}")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerResponse(StatusCodes.Status204NoContent, typeof(Unit))]
        [SwaggerResponse(StatusCodes.Status404NotFound, typeof(NotFoundObjectResult))]
        public async Task<ActionResult<Unit>> UpdateProfile([FromRoute] int id, [FromBody] UpdateUserProfileCommand command)
        {
            if (id != command.Id)
                return BadRequest();
            
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpPut("{id:int}/avatar")]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        public async Task<IActionResult> ChangeAvatar([FromRoute] int id, [FromBody] FileUpload file)
        {
            var extension = ".png";
            var imageDataByteArray = Convert.FromBase64String(file.FileData);
            var imageDataStream = new MemoryStream(imageDataByteArray);
            imageDataStream.Position = 0;
            
            var command = new UpdateUserAvatarCommand(id, extension, imageDataStream);
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpGet("{id:int}/avatar")]
        [ProducesResponseType(typeof(FileContentResult), 200)]
        [SwaggerResponse(200, typeof(FileContentResult))]
        public async Task<FileContentResult> GetAvatar([FromRoute] int id)
        {
            return await Mediator.Send(new GetAvatarQuery(id));
        }
    }
}