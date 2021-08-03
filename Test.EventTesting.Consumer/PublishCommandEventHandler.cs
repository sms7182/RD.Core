using NServiceBus;
using NServiceBus.Logging;
using RD.ConsumerService.CommandExample;
using RD.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.ConsumerService.EventExample
{
    public class PublishCommandEventHandler : BaseEventHandler<PublishCommandEvent>
    {
        static readonly ILog log = LogManager.GetLogger<PublishCommandEventHandler>();

        public override Task Handle(PublishCommandEvent message)
        {
            log.Info($"Event Message Receipt in 444444:{message.Id}");
            return Task.CompletedTask;
        }

      
    }


}
