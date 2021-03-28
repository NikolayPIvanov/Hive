using System.Collections.Generic;
using System.Threading.Tasks;
using Hive.Gig.Application.GigPackages.Commands;
using Hive.Gig.Application.GigPackages.Queries;
using Hive.Gig.Application.Gigs.Commands;
using Hive.Gig.Application.Gigs.Queries;
using Hive.Gig.Application.GigScopes.Command;
using Hive.Gig.Application.GigScopes.Queries;
using Hive.Gig.Application.Questions.Commands;
using Hive.Gig.Application.Questions.Queries;
using Hive.Gig.Contracts.Objects;
using Hive.Gig.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Hive.LooselyCoupled.Controllers
{
    public class GigsController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<GigDto>> Get([FromRoute] int id)
        {
            var gig = await Mediator.Send(new GetGigQuery(id));
            return Ok(gig);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateGigCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
       
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateGigCommand command)
        { 
            if (id != command.Id)
            {
                return BadRequest();
            }
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteGigCommand(id));
            return NoContent();
        }
        
        
        [HttpGet("{id}/scopes")]
        public async Task<ActionResult<GigScopeDto>> GetScope([FromRoute] int id)
        {
            var scope = await Mediator.Send(new GetGigScopeQuery(id));
            return Ok(scope);
        }
        
        [HttpGet("{id}/scopes/{scopeId}")]
        public async Task<ActionResult<GigScopeDto>> GetScope([FromRoute] int id, int scopeId)
        {
            var scope = await Mediator.Send(new GetScopeByIdQuery(id, scopeId));
            return Ok(scope);
        }
        
        [HttpPost("{id}/scopes")]
        public async Task<ActionResult<int>> CreateScope(int id, [FromBody] CreateGigScopeCommand command)
        {
            if (id != command.GigId)
            {
                return BadRequest();
            }

            var scopeId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetScope), new { id, scopeId}, new {id,scopeId});
        }
        
        [HttpPut("{id}/scopes")]
        public async Task<IActionResult> UpdateDescription(int id, [FromBody] UpdateGigScopeCommand command)
        {
            if (id != command.GigId)
            {
                return BadRequest();
            }

            await Mediator.Send(command);
            return NoContent();
        }
        
        
        [HttpGet("{id}/packages")]
        public async Task<ActionResult<IEnumerable<PackageDto>>> GetPackages(int id)
        {
            var packages = await Mediator.Send(new GetGigPackagesQuery(id));
            return Ok(packages);
        }
        
        [HttpGet("{id}/packages/{packageId}")]
        public async Task<ActionResult<PackageDto>> GetPackage(int id, int packageId)
        {
            var package = await Mediator.Send(new GetPackageQuery(packageId));
            return Ok(package);
        }
        
        // TODO: Add restriction on the type of package can be created for a given gig
        [HttpPost("{id}/packages")]
        public async Task<IActionResult> CreatePackage(int id, [FromBody] CreatePackageCommand command)
        {
            if (id != command.GigId)
            {
                return BadRequest();
            }
            
            var packageId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetPackage), new {id, packageId}, new {id, packageId});
        }
        
        [HttpPut("{id}/packages/{packageId}")]
        public async Task<IActionResult> UpdatePackage(int id, int packageId, [FromBody] UpdatePackageCommand command)
        {
            if (packageId != command.Id)
            {
                return BadRequest();
            }
            
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpDelete("{id}/packages/{packageId}")]
        public async Task<IActionResult> DeletePackage(int id, int packageId)
        {
            await Mediator.Send(new DeletePackageCommand(packageId));
            return NoContent();
        }
        
        
        [HttpGet("{id}/questions")]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions(int id)
        {
            var questions = await Mediator.Send(new GetQuestionsQuery(id));
            return Ok(questions);
        }
        
        [HttpGet("{id}/questions/{questionId}")]
        public async Task<ActionResult<QuestionDto>> GetQuestion(int id, int questionId)
        {
            var question = await Mediator.Send(new GetQuestionByIdQuery(id, questionId));
            return Ok(question);
        }
        
        [HttpPost("{id}/questions")]
        public async Task<IActionResult> CreateQuestion(int id, [FromBody] CreateQuestionCommand command)
        {
            if (id != command.GigId)
            {
                return BadRequest();
            }
            
            var questionId = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetQuestion), new {id, questionId}, new {id, questionId});
        }
        
        [HttpPut("{id}/questions/{questionId}")]
        public async Task<IActionResult> UpdateQuestion(int id, int questionId, [FromBody] UpdateQuestionCommand command)
        {
            if (questionId != command.Id)
            {
                return BadRequest();
            }
            
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpDelete("{id}/questions/{questionId}")]
        public async Task<IActionResult> DeleteQuestion(int id, int questionId)
        {
            await Mediator.Send(new DeleteQuestionCommand(questionId));
            return NoContent();
        }
    }
}