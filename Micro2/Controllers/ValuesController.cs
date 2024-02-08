using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            throw new Exception("hata var");
            return Ok("Micro2");
        }
    }
}