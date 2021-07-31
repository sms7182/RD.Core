using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NServiceBus;
using RD.Core.Messaging;
using RDNameSpace;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RD.Core.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        static int messagesSent;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBus _bus;


        public WeatherForecastController(IBus bus, ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
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
        public async Task<ActionResult> SendCommand()
        {
            string orderId = Guid.NewGuid().ToString().Substring(0, 8);

            var command = new ClientPublishCommand();

            await _bus.Send(command);
            _logger.LogInformation($"Sending PlaceOrder, OrderId = {orderId}");

            dynamic model = new ExpandoObject();
            model.OrderId = orderId;
            model.MessagesSent = Interlocked.Increment(ref messagesSent);
            return Ok();
        }                
    }
}
