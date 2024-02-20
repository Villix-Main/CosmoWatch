using Microsoft.AspNetCore.Mvc;

namespace CosmoWatchControllerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorController : ControllerBase
    {
        [HttpGet("getNames")]
        public ActionResult GetSensorNames()
        {
            return Ok("oxygen,temperature,carbon_dioxide");
        }
    }
}
