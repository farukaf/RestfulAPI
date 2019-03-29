using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RestfulAPI.Controllers
{
    /// <summary>
    /// Just to check if is blocking unauthorized access
    /// </summary>
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var ret = "Autorized!!";

            return Ok(ret);
        }
    }
}