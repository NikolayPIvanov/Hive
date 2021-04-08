using System.Threading.Tasks;
using Hive.Application.GigsManagement.GigPackages.Commands.CreatePackage;
using Hive.Application.GigsManagement.GigPackages.Queries;
using Hive.Application.GigsManagement.GigPackages.Queries.GetPackage;
using Microsoft.AspNetCore.Mvc;

namespace Hive.WebUI.Controllers
{
    public class PackagesController : ApiControllerBase
    {
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PackageDto>> Get(int id)
        {
            var entity = await Mediator.Send(new GetPackageQuery() {Id = id});
            return Ok(entity);
        }

        [HttpPost]
        public async Task<ActionResult<PackageDto>> Post([FromBody] CreatePackageCommand command)
        {
            var id = await Mediator.Send(command);
            return CreatedAtAction(nameof(Get), new {id}, id);
        }
    }
}