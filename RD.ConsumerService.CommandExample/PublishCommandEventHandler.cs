using NServiceBus;
using NServiceBus.Logging;
using RD.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.ConsumerService.CommandExample
{
    public class PublishCommandEventHandler : BaseEventHandler<PublishCommandEvent>
    {
        static readonly ILog log = LogManager.GetLogger<PublishCommandEventHandler>();
        public override Task Handle(PublishCommandEvent message)
        {
            log.Info($"Event Message Receipt in CommandProject:{message.Id}");
            return Task.CompletedTask;
        }

      
    }
}
