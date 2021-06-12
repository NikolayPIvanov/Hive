using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Hive.Common.Core.Models;
using Hive.Investing.Application.Investments.Commands;
using Hive.Investing.Application.Investments.Queries;
using Hive.Investing.Application.Plans.Commands;
using Hive.Investing.Application.Plans.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace Investing.Management.Controllers
{
    [Authorize(Roles = "Seller, Investor")]
    public class PlansController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PlanDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<ActionResult<PlanDto>> GetPlan([FromRoute] int id, CancellationToken cancellationToken = default) 
            => Ok(await Mediator.Send(new GetPlanByIdQuery(id), cancellationToken));
        
        [HttpGet]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<PlanDto>), Description = "Successful operation")]
        public async Task<ActionResult<PlanDto>> GetPlans([FromQuery] int pageNumber = 1, int pageSize = 10, string key = null, CancellationToken cancellationToken = default) 
            => Ok(await Mediator.Send(new GetPlansQuery(pageNumber, pageSize, key), cancellationToken));
        
        [HttpGet("random")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<PlanDto>), Description = "Successful operation")]
        public async Task<ActionResult<PlanDto>> GetRandomPlans([FromQuery] PaginatedQuery query, string key = null, CancellationToken cancellationToken = default) 
            => Ok(await Mediator.Send(new GetRandomPlansQuery(query, key), cancellationToken));


        [HttpPost]
        [Authorize(Roles = "Seller")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(PlanDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestResult), Description = "Bad Request operation")]
        public async Task<ActionResult<PlanDto>> CreatePlan([FromBody] CreatePlanCommand command,
            CancellationToken cancellationToken = default)
        {
            var plan = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetPlan), new {id = plan.Id}, plan);
        }
        
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Seller")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestResult), Description = "Bad Request operation")]
        public async Task<IActionResult> UpdatePlan([FromRoute] int id, [FromBody] UpdatePlanCommand command,
            CancellationToken cancellationToken = default)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            
            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
        
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Seller")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<IActionResult> DeletePlan([FromRoute] int id, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeletePlanCommand(id), cancellationToken);
            return NoContent();
        }

        [HttpGet("{planId:int}/investments")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(PaginatedList<InvestmentDto>), Description = "Successful operation")]
        public async Task<ActionResult<PaginatedList<InvestmentDto>>> GetPlanInvestments([FromRoute] int planId, [FromQuery] int pageSize = 10, int pageNumber = 1, bool onlyAccepted = false, CancellationToken cancellationToken = default)
            => Ok(await Mediator.Send(new GetInvestmentsByPlanQuery(planId, pageNumber, pageSize, onlyAccepted), cancellationToken));
        
        [HttpGet("{planId:int}/investments/{investmentId:int}")]
        [SwaggerResponse(HttpStatusCode.OK, typeof(InvestmentDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<ActionResult<InvestmentDto>> GetInvestment([FromRoute] int planId, int investmentId, CancellationToken cancellationToken)
            => Ok(await Mediator.Send(new GetInvestmentByIdQuery(planId, investmentId), cancellationToken));
        
        [HttpPost("{planId:int}/investments")]
        [Authorize(Roles = "Investor")]
        [SwaggerResponse(HttpStatusCode.Created, typeof(InvestmentDto), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestObjectResult), Description = "Bad request operation")]
        public async Task<ActionResult<InvestmentDto>> MakeInvestment([FromRoute] int planId, [FromBody] MakeInvestmentCommand command,
            CancellationToken cancellationToken)
        {
            var id = await Mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(GetInvestment), new { planId, investmentId = id.Id}, id);
        }
        
        [HttpPut("{planId:int}/investments/{investmentId:int}")]
        [Authorize(Roles = "Investor")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestResult), Description = "Bad Request operation")]
        public async Task<IActionResult> UpdateInvestment([FromRoute] int planId, int investmentId, [FromBody] UpdateInvestmentCommand command,
            CancellationToken cancellationToken)
        {
            // TODO: Should return 404 if planId is not found
            if (investmentId != command.InvestmentId)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
        
        
        [HttpDelete("{planId:int}/investments/{investmentId:int}")]
        [Authorize(Roles = "Investor")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        public async Task<IActionResult> DeleteInvestment([FromRoute] int planId, int investmentId, CancellationToken cancellationToken)
        {
            await Mediator.Send(new DeleteInvestmentCommand(planId, investmentId), cancellationToken);
            return NoContent();
        }
    
        [HttpPut("{planId:int}/investments/{investmentId:int}/processing")]
        [Authorize(Roles = "Seller")]
        [SwaggerResponse(HttpStatusCode.NoContent, typeof(IActionResult), Description = "Successful operation")]
        [SwaggerResponse(HttpStatusCode.NotFound, typeof(NotFoundObjectResult), Description = "Not Found operation")]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(BadRequestResult), Description = "Bad Request operation")]
        public async Task<IActionResult> ProcessInvestment([FromRoute] int planId, int investmentId, [FromBody] ProcessInvestmentCommand command,
            CancellationToken cancellationToken)
        {
            if (investmentId != command.InvestmentId || planId != command.PlanId)
            {
                return BadRequest();
            }

            await Mediator.Send(command, cancellationToken);
            return NoContent();
        }
    }
}