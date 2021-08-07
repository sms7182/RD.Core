using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using NServiceBus;
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
      
        public static IServiceCollection AddNServiceBus(this IServiceCollection services,string endPoint,Dictionary<string,KeyValuePair<Type,string>> destionationEndpoints,string rabbitConnection,string mongoConfiguration)
        {
            AddNServiceBus(services, endPoint, endPointConfiguration =>
            {
                var transporter = endPointConfiguration.UseTransport<RabbitMQTransport>();


                transporter.UseConventionalRoutingTopology();
                transporter.ConnectionString(rabbitConnection);
                var routing=transporter.Routing();
                foreach (var typedic in destionationEndpoints)
                {
                    routing.RouteToEndpoint(typedic.Value.Key.Assembly,typedic.Key, typedic.Value.Value);
                }
                
            },mongoConfiguration);

            return services;
        }
      
        
        
        public static IServiceCollection AddNServiceBusPublisherEvent(this IServiceCollection services,string publisherEndpoint,string rabbitConnectionString,string mongoConfiguration)
        {
           var endPointConfiguration= new EndpointConfiguration(publisherEndpoint);
            endPointConfiguration.EnableInstallers();
            var transporter=  endPointConfiguration.UseTransport<RabbitMQTransport>();
            transporter.UseConventionalRoutingTopology();
            transporter.ConnectionString(rabbitConnectionString);
            
            AddNServiceBus(services,publisherEndpoint,endPointConfigure=> { 
                endPointConfigure.EnableInstallers();
                endPointConfigure.UseTransport<RabbitMQTransport>()
                                 .UseConventionalRoutingTopology()
                                 .ConnectionString(rabbitConnectionString); 
            },mongoConfiguration);      
       
       
            return services;
        }
       
        static IServiceCollection AddNServiceBus(this IServiceCollection services,string endPoint,Action<EndpointConfiguration> configuration, string mongoConfiguration)
        {
            var endPointConfiguration = new EndpointConfiguration(endPoint);
            endPointConfiguration.EnableInstallers();
            configuration(endPointConfiguration);

            if (!string.IsNullOrWhiteSpace(mongoConfiguration))
                endPointConfiguration.UsePersistence<MongoPersistence>().MongoClient(new MongoClient(mongoConfiguration)).UseTransactions(false).DatabaseName("SagaDB");
         else   endPointConfiguration.UsePersistence<InMemoryPersistence>();
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
