using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.Api.Controllers
{
    [Route("/api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet("moderator")]
        [Authorize(Policy = "Moderator")]
        public ActionResult GetModerator()
        {
            return Ok();
        }

        [HttpGet("admin")]
        [Authorize(Policy = "Admin")]
        public ActionResult GetAdmin()
        {
            return Ok();
        }

        [HttpGet("ban")]
        [Unbanned]
        public ActionResult GetUnbanned()
        {
            return Ok();
        }
    }
}
