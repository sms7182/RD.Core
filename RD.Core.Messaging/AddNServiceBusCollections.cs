using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Persistence.MongoDB;
using NServiceBus.Transport.RabbitMQ;
using RD.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RD.Core.Messaging
{
    public static class AddNServiceBusCollections
    {
      
        public static IServiceCollection AddNServiceBus(this IServiceCollection services, Type type,string assembly,string endPoint,string destinationEndPoint,string rabbitConnection)
        {

            AddNServiceBus(services, endPoint,endPointConfiguration =>
             {
                 var transporter = endPointConfiguration.UseTransport<RabbitMQTransport>();
                 

                 transporter.UseConventionalRoutingTopology();
                 transporter.ConnectionString(rabbitConnection);
                 transporter.Routing().RouteToEndpoint(type.Assembly, assembly, destinationEndPoint);
                
                 //endPointConfiguration.UseTransport<LearningTransport>().Routing().RouteToEndpoint(type.Assembly,assembly,destinationEndPoint);
             });
            
            return services;

        }
        public static IServiceCollection AddNServiceBus(this IServiceCollection services,Type type,string endpoint,string destinationEndpoint,string rabbitConnection)
        {
            AddNServiceBus(services, endpoint, endPointConfiguration =>
            {
                var transporter = endPointConfiguration.UseTransport<RabbitMQTransport>();

                transporter.UseConventionalRoutingTopology();
                transporter.ConnectionString(rabbitConnection);
                transporter.Routing().RouteToEndpoint(type.Assembly, destinationEndpoint);
            });
            return services;
        }
        static IServiceCollection AddNServiceBus(this IServiceCollection services, string endPoint, Action<EndpointConfiguration> configuration)
        {
            var endPointConfiguration = new EndpointConfiguration(endPoint);
            endPointConfiguration.EnableInstallers();
            configuration(endPointConfiguration);

            services.AddNServiceBus(endPointConfiguration);
            return services;

        }
        public static IServiceCollection AddNServiceBusPublisherEvent(this IServiceCollection services,string publisherEndpoint,string rabbitConnectionString)
        {
           var endPointConfiguration= new EndpointConfiguration(publisherEndpoint);
            endPointConfiguration.EnableInstallers();
            var transporter=  endPointConfiguration.UseTransport<RabbitMQTransport>();
            transporter.UseConventionalRoutingTopology();
            transporter.ConnectionString(rabbitConnectionString);
            
            AddNServiceBus(services, endPointConfiguration);      
       
       
            return services;
        }
       
        static IServiceCollection AddNServiceBus(this IServiceCollection services, EndpointConfiguration endPointConfiguration)
        {
            //  endPointConfiguration.UsePersistence<MongoDbPersistence>().SetConnectionString("");
            endPointConfiguration.UsePersistence<InMemoryPersistence>();
              services.AddSingleton(endPointConfiguration);
            
            services.AddSingleton(new SessionAndConfigurationHolder(endPointConfiguration));
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
            
            services.AddSingleton<IBus, Bus>();
            
            return services;
        }
        
    }
}
