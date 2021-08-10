using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using NServiceBus;
using RD.Core.Messaging;
using System;
using System.Threading;
using System.Threading.Tasks;



namespace RD.ConsumerService.CommandExample
{

    public class ServiceBusHosted : NServiceBusService
    {
        public ServiceBusHosted(SessionAndConfigurationHolder sessionAndConfigurationHolder):base(sessionAndConfigurationHolder)
        {
            
        }

        public  async Task Start(CancellationToken cancellationToken)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<IExampleService, ExampleService>();
          var startedEndPoint=  EndpointWithExternallyManagedContainer.Create(holder.EndpointConfiguration, (NServiceBus.ObjectBuilder.IConfigureComponents)services);
            var provider=services.BuildServiceProvider();
           await startedEndPoint.Start(provider);
        }
    }

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

                 services.AddScoped<IExampleService, ExampleService>();
                 var endPointConfiguration = new EndpointConfiguration(nservisBusSecret.EndpointHost);
                 endPointConfiguration.EnableInstallers();
                 endPointConfiguration.UsePersistence<MongoPersistence>().MongoClient(new MongoClient("mongodb://localhost:27017")).UseTransactions(false).DatabaseName("SagaDB");
                 endPointConfiguration.UseTransport<RabbitMQTransport>()
                 .UseConventionalRoutingTopology()
                  .ConnectionString(nservisBusSecret.RabbitConnectionInfo);

                 //hostContext.Contains(c =>
                 //{
                 //    c.Register(Component.For<Lazy<IMessageSession>>().Instance(((IStartableEndpointWithExternallyManagedContainer)_endpoint).MessageSession).LifestyleSingleton());

                 //    // Resolve service
                 //    _service = c.Resolve<MyService>();
                 //});
               
                 var statrEndpoint = EndpointWithExternallyManagedServiceProvider.Create(endPointConfiguration, services);
                 services.AddSingleton(p => statrEndpoint.MessageSession.Value);

                 services.AddSingleton<IBus, Bus>();
                 var provider = services.BuildServiceProvider();

                 statrEndpoint.Start(provider).Wait();
                // services.AddNServiceBusPublisherEvent(nservisBusSecret.EndpointHost, nservisBusSecret.RabbitConnectionInfo, "mongodb://localhost:27017");


             });

        }
    }
}
