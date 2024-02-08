using Micro1.Models;
using Micro1.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Micro1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(Micro2Services micro2Services, AppDbContext appDbContext) : ControllerBase
    {
        private readonly Micro2Services _micro2Services = micro2Services;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _micro2Services.GetMicro2();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            appDbContext.Products.Add(new Product() { Name = "kalem 1", Price = 100 });
            await appDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}