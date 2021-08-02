using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RD.ConsumerService.SagaExample.Events;
using RD.Core.Messaging;
using System;
using System.Threading.Tasks;

namespace RD.ConsumerService.SagaExample
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
            Console.WriteLine("Hello World!");
            await CreateHostBuilder(args).RunConsoleAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                  .CreateDefaultBuilder(args)
                  .ConfigureAppConfiguration((hostContext, builder) => { builder.AddUserSecrets<NServiceBusSecret>(); })
                  .ConfigureServices((hostContext,services)=> 
                  {
                     var nserviceBusSecret= hostContext.Configuration.GetSection(typeof(NServiceBusSecret).Name).Get<NServiceBusSecret>();
                      services.AddNServiceBusPublisherEvent(nserviceBusSecret.EndpointHost,  nserviceBusSecret.RabbitConnectionInfo);
                    
                  });

        }

    }
}
