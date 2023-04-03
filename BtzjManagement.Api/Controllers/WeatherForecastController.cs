using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BtzjManagement.Api.Filter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BtzjManagement.Api.Controllers
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
        [Encryption]
        public IEnumerable<WeatherForecast> Get(string IDCard, string cityCode, string machineNumber)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost]
        [Encryption]
        public string Post([FromBody] PostParms p)
        {
            var RequestBody = new StreamReader(this.Request.BodyReader.AsStream()).ReadToEnd();
            string body1 = null;
            //  这句很重要，开启读取 否者下面设置读取为0会失败
            using (var reader = new System.IO.StreamReader(this.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                body1 = reader.ReadToEnd();
            }
            Request.Body.Position = 0;
            string body2 = null;
            //  这句很重要，开启读取 否者下面设置读取为0会失败
            using (var reader = new System.IO.StreamReader(this.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                body2 = reader.ReadToEnd();
            }

            return p.IDCard + "|" + p.cityCode;
        }
        [HttpGet("unencrypted")]
        public IEnumerable<WeatherForecast> Get2(string IDCard, string cityCode, string machineNumber)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost("unencrypted")]
        public string Post2([FromBody] PostParms p)
        {
            var RequestBody = new StreamReader(this.Request.BodyReader.AsStream()).ReadToEnd();
            string body1 = null;
            //  这句很重要，开启读取 否者下面设置读取为0会失败
            using (var reader = new System.IO.StreamReader(this.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                body1 = reader.ReadToEnd();
            }
            Request.Body.Position = 0;
            string body2 = null;
            //  这句很重要，开启读取 否者下面设置读取为0会失败
            using (var reader = new System.IO.StreamReader(this.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                body2 = reader.ReadToEnd();
            }

            return p.IDCard + "|" + p.cityCode;
        }
    }
    public class PostParms
    {
        public string IDCard { get; set; }
        public string cityCode { get; set; }
    }
}

