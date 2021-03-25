using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hive.LooselyCoupled.Controllers
{
    [Microsoft.AspNetCore.Components.Route("identity")]
    [Authorize]
    public class IdentityController : ApiControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}