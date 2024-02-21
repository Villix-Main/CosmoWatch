using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CosmoWatchControllerAPI.Controllers
{
    public class SensorNames
    {
        public string? names { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class SensorController : ControllerBase
    {
        private string? mSensorNames = string.Empty;

        [HttpGet("getNames")]
        public ActionResult GetSensorNames()
        {
            return Ok("temperature,oxygen,carbon_dioxide");
        }

        [HttpPost("sendNames")]
        public ActionResult SendSensorNames([FromBody] SensorNames body)
        {
            mSensorNames = body.names;

            return Ok();
        }
    }
}
