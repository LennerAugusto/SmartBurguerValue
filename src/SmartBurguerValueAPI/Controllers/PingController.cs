using Microsoft.AspNetCore.Mvc;

namespace SmartBurguerValueAPI.Controllers
{
    public class PingController : ControllerBase
    {
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("API está rodando!");
        }
    }
}
