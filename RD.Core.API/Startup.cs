using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NServiceBus;
using SagaNameSpace;
using RD.Core.Messaging;
using ShareNameSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD.Core.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var nserviceBusSecret = Configuration.GetSection(typeof(NServiceBusSecret).Name)
                .Get<NServiceBusSecret>();
            Dictionary<string, KeyValuePair<Type, string>> keyValuePairs = new Dictionary<string, KeyValuePair<Type, string>>();
            keyValuePairs.Add("ShareNameSpace", new KeyValuePair<Type, string>(typeof(ClientPublishCommand), nserviceBusSecret.DestinationEndpointHost));
            keyValuePairs.Add("SagaNameSpace",new KeyValuePair<Type, string>(typeof(SagaStartingCommand), nserviceBusSecret.SagaEndpointHost));
            services

             //.AddNServiceBus(typeof(ClientPublishCommand), "ShareNameSpace", nserviceBusSecret.EndpointHost, nserviceBusSecret.DestinationEndpointHost, nserviceBusSecret.RabbitConnectionInfo)
             // .AddNServiceBus(typeof(SagaStartingCommand),"SagaNameSpace",nserviceBusSecret.EndpointHost,nserviceBusSecret.SagaEndpointHost,nserviceBusSecret.RabbitConnectionInfo);
             .AddNServiceBus(nserviceBusSecret.EndpointHost,keyValuePairs,nserviceBusSecret.RabbitConnectionInfo, "mongodb://localhost:27017/SagaDB");

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RD.Core.API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "RD.Core.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
