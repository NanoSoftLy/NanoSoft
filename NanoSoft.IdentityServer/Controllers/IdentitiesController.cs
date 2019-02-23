using Microsoft.AspNetCore.Mvc;

namespace NanoSoft.IdentityServer.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class IdentitiesController : ControllerBase
    {
        //private readonly IdentityService<IdentityDbContext, IdentityUser> _service;

        //public IdentitiesController(IdentityService service)
        //{
        //    _service = service;
        //}

        //[HttpPost("seed")]
        //public async Task<ActionResult> SeedAsync([FromBody] IdentityModel model)
        //{
        //    Console.WriteLine("seeding");

        //    if (await _service.AnyAsync())
        //        return BadRequest();

        //    await _service.CreateIdentityAsync(model.UserId, "nanosoft");

        //    return Ok();
        //}
    }
}