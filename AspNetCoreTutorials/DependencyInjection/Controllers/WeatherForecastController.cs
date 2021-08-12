using DependencyInjection.TestLifetime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjection.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get(
            [FromServices] ITestTransient testTransient1, // 在参数中通过增加[FromService]对参数进行注入
            [FromServices] ITestTransient testTransient2,
            [FromServices] ITestScoped testScoped1,
            [FromServices] ITestScoped testScoped2,
            [FromServices] ITestSignleton testSignleton1,
            [FromServices] ITestSignleton testSignleton2
            )
        {
            Console.WriteLine($"--------------请求开始--------------");
            Console.WriteLine($"Transient1 HashCode：{testTransient1.GetHashCode()}");
            Console.WriteLine($"Transient2 HashCode：{testTransient2.GetHashCode()}");
            Console.WriteLine($"ITestScoped1 HashCode：{testScoped1.GetHashCode()}");
            Console.WriteLine($"ITestScoped2 HashCode：{testScoped2.GetHashCode()}");
            Console.WriteLine($"ITestSignleton1 HashCode：{testSignleton1.GetHashCode()}");
            Console.WriteLine($"ITestSignleton2 HashCode：{testSignleton2.GetHashCode()}");
            Console.WriteLine($"--------------请求结束--------------");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("get2")]
        public IEnumerable<WeatherForecast> Get2(
            [FromServices] OneSelfClass testClass
            )
        {
            testClass.SayHello();

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
