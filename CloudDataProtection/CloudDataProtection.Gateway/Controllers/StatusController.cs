using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Controllers
{
    [ApiController]
    [Route("")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public ActionResult Status()
        {
            return Ok("CloudDataProtection");
        }
    }
}