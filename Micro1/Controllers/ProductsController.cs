using Micro1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(Micro2Services micro2Services) : ControllerBase
    {
        private readonly Micro2Services _micro2Services = micro2Services;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _micro2Services.GetMicro2();
            return Ok(result);
        }
    }
}