using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RD.Core.Client.Configuration
{
    public static class AddNServiceBusCollections
    {
        public static IServiceCollection AddNServiceBus(this IServiceCollection services, string endPoint, Action<EndpointConfiguration> configuration)
        {
            var endPointConfiguration = new EndpointConfiguration(endPoint);

            configuration(endPointConfiguration);

            services.AddNServiceBus(endPointConfiguration);
            return services;

        }
       
        static IServiceCollection AddNServiceBus(this IServiceCollection services, EndpointConfiguration configuration)
        {
            services.AddSingleton(configuration);
            services.AddSingleton(new SessionAndConfigurationHolder(configuration));
            services.AddHostedService<NServiceBusService>();
            services.AddSingleton(provider =>
            {
                var management = provider.GetService<SessionAndConfigurationHolder>();
                if (management.MessageSession != null)
                {
                    return management.MessageSession;
                }

                var timeout = TimeSpan.FromSeconds(30);
                if (!SpinWait.SpinUntil(() => management.MessageSession != null || management.StartupException != null,
                    timeout))
                {
                    throw new TimeoutException($"Unable to resolve the message session within '{timeout.ToString()}'. If you are trying to resolve the session within hosted services it is encouraged to use `Lazy<IMessageSession>` instead of `IMessageSession` directly");
                }

                management.StartupException?.Throw();

                return management.MessageSession;
            });
            services.AddSingleton(provider => new Lazy<IMessageSession>(provider.GetService<IMessageSession>));
            return services;
        }
    }
}
