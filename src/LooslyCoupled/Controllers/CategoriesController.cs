﻿using System.Threading.Tasks;
using Gig.Contracts;
using Hive.Gig.Application.Categories.Commands;
using Hive.Gig.Application.Categories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LooslyCoupled.Controllers
{
    public class CategoriesController : ApiControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> Get([FromRoute] int id)
        {
            var category = await Mediator.Send(new GetCategoryQuery(id));
            return Ok(category);
        }
        
        [HttpGet]
        public async Task<ActionResult<CategoryDto>> Get([FromQuery] GetCategoriesQuery query)
        {
            var list = await Mediator.Send(query);
            return Ok(list);
        }
        
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateCategoryCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new { id }, id);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("Body and route ids mismatch.");
            }
            
            await Mediator.Send(command);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}