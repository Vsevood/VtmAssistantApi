using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VtmApiController : ControllerBase
    {
        private readonly ILogger<VtmApiController> _logger;

        public VtmApiController(ILogger<VtmApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Get()
        {
            Console.WriteLine("123");
            return Ok();
        }
    }
}