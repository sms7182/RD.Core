﻿using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace RD.Core.Messaging
{
    class NServiceBusService : IHostedService
    {
        public NServiceBusService(SessionAndConfigurationHolder holder)
        {
            this.holder = holder;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                endpoint = await Endpoint.Start(holder.EndpointConfiguration)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                holder.StartupException = ExceptionDispatchInfo.Capture(e);
                return;
            }

            holder.MessageSession = endpoint;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (endpoint != null)
                {
                    await endpoint.Stop()
                        .ConfigureAwait(false);
                }
            }
            finally
            {
                holder.MessageSession = null;
                holder.StartupException = null;
            }
        }

        readonly SessionAndConfigurationHolder holder;
        IEndpointInstance endpoint;
    }
    public class SessionAndConfigurationHolder
    {
        public SessionAndConfigurationHolder(EndpointConfiguration endpointConfiguration)
        {
            EndpointConfiguration = endpointConfiguration;
        }

        public EndpointConfiguration EndpointConfiguration { get; }

        public IMessageSession MessageSession { get; internal set; }

        public ExceptionDispatchInfo StartupException { get; internal set; }
    }
}
