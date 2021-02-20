using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Seller.Contracts;

namespace Api.Controllers
{
    public class GigModel
    {
        public string Title { get; set; }
        public List<string> Tags { get; set; }
    }
    
    [Controller]
    [Route("api/[controller]")]
    public class GigController : ControllerBase
    {
        private readonly ISellerRepository _sellerRepository;
        private readonly ILogger<GigController> _logger;

        public GigController(ISellerRepository sellerRepository ,ILogger<GigController> logger)
        {
            _sellerRepository = sellerRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _sellerRepository.GetGigsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var gig = await _sellerRepository.GetGigAsync(id);
            return Ok(gig);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]GigModel model)
        {
            var id = await _sellerRepository.CreateGigAsync(model.Title, model.Tags);
            return CreatedAtAction(nameof(Get), new {id}, id);
        }
    }
}