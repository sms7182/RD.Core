using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.ConsumerService.CommandExample
{
    public static  class AddNServiceBusCollections
    {
        public static IServiceCollection AddNServiceBus(this IServiceCollection services,string endPoint, Action<EndpointConfiguration> configuration)
        {
            var endPointConfiguration = new EndpointConfiguration(endPoint);
            configuration(endPointConfiguration);
            return services;

        }
    }
}
