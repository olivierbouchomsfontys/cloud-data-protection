using Microsoft.AspNetCore.Mvc;

namespace CloudDataProtection.Controllers
{
    [ApiController]
    [Route("")]
    public class StatusController : ControllerBase
    {
        [Route("")]
        public ActionResult Status()
        {
            return Ok("CloudDataProtection");
        }
    }
}