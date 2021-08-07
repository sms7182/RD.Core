using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using RD.Core.Messaging;
using System;
using System.Threading.Tasks;



namespace RD.ConsumerService.CommandExample
{


        public class NServiceBusSecret
    {
        public string EndpointHost { get; set; }
        public string DestinationEndpointHost { get; set; }
        public string RabbitConnectionInfo { get; set; }

    }

class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "CommandHanlder";
           
            await CreateHostBuilder(args).RunConsoleAsync();
        }


        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, builder) => { builder.AddUserSecrets<NServiceBusSecret>(); })
             .ConfigureServices((hostContext, services) =>
            {
                var nservisBusSecret = hostContext.Configuration.GetSection(typeof(NServiceBusSecret).Name).Get<NServiceBusSecret>();
                services.AddNServiceBusPublisherEvent(nservisBusSecret.EndpointHost,nservisBusSecret.RabbitConnectionInfo, "mongodb://localhost:27017");
                services.AddScoped<IBus, Bus>();

            });
            
        }
    }
}
