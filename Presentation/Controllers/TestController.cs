using Infrastructure;

using Microsoft.AspNetCore.Mvc;


namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController() : ControllerBase
    {

        [HttpGet]
        public int Get(int max)
        {
            var random = new Random();
            return random.Next(max);
        }
    }
}
