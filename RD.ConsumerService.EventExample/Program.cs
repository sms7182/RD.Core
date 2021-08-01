﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using RD.ConsumerService.CommandExample;
using RD.Core.Messaging;
using System;
using System.Threading.Tasks;

namespace RD.ConsumerService.EventExample
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
            Console.Title = "EventHandler";
            await CreateHostBuilder(args).RunConsoleAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostContext, builder) => { builder.AddUserSecrets<NServiceBusSecret>(); })
             .ConfigureServices((hostContext, services) =>
             {
                 var nservisBusSecret = hostContext.Configuration.GetSection(typeof(NServiceBusSecret).Name).Get<NServiceBusSecret>();
                 services.AddNServiceBusPublisherEvent(nservisBusSecret.EndpointHost, typeof(PublishCommandEvent),nservisBusSecret.RabbitConnectionInfo);
                 services.AddScoped<IBus, Bus>();

             });
              //.UseNServiceBus(context =>
              //{
              //    var nserviceBusSection = context.Configuration.GetSection("NServiceBusSecret").Get<NServiceBusSecret>();

              //    var endpointConfiguration = new EndpointConfiguration(nserviceBusSection.EndpointHost);

              //    endpointConfiguration.UseTransport<LearningTransport>();

              //    endpointConfiguration.SendFailedMessagesTo("error");
              //    endpointConfiguration.AuditProcessedMessagesTo("audit");
              //    endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");

              //    // So that when we test recoverability, we don't have to wait so long
              //    // for the failed message to be sent to the error queue
              //    var recoverablility = endpointConfiguration.Recoverability();
              //    recoverablility.Delayed(
              //        delayed =>
              //        {
              //            delayed.TimeIncrease(TimeSpan.FromSeconds(2));
              //        }
              //    );

              //    var metrics = endpointConfiguration.EnableMetrics();
              //    metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromMilliseconds(500));

              //    return endpointConfiguration;
              //});
        }
    }
}
