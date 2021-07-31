using NServiceBus;
using System;
using System.Threading.Tasks;

namespace RD.Core.Messaging
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Mihato";
            var endpointConfiguration = new EndpointConfiguration("CoreMessaging");
            endpointConfiguration.UseTransport<LearningTransport>();

           var endpointInstance=  await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
           await endpointInstance.Stop().ConfigureAwait(false);


        }

       
    }
}
