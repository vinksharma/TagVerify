using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace TagVerify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static readonly string[] Urls = new[]
        {
            "https://www.ntag.coinworldplus.com/verify?enc=F0B81E98D005974C13FA8003B99AE04Ext=7EC3BD99ED1769D173D847E7E4503231x611C3BD723352B0D",
            "https://www.ntag.coinworldplus.com/verify?enc=72912F5CEE7FD1723880F6EF4F22A7CExt=5F9A8B84C68ADE91AC4730261979D36Dx945E23A00BD01D3F",
            "https://www.ntag.coinworldplus.com/verify?enc=62C6C09C38E4E8AC103731BAAA49F573xt=BD6BE310D57320BA7F658590277C8FF6xC3B1C99BA3A95302",
            "https://www.ntag.coinworldplus.com/verify?enc=4337E07B8E47E65DC06A8D4A4F70CBD7xt=68240DD07A398CB5BDE03A283AFA565Ex3BB6C1561648511C",
            "https://www.ntag.coinworldplus.com/verify?enc=0C34A2DCEF50D86268198AA99AFFDC58xt=9412047DEB80EFAEC47290273E596A18xEDAB02AB9CB026C3"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)],
                DecodedUrl = NTagLibWrapper.ParseTagUrl(Urls[rng.Next(Urls.Length)]).UId
            })
            .ToArray();
        }
    }
}
