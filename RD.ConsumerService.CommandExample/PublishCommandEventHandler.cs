using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.ConsumerService.CommandExample
{
    public class PublishCommandEventHandler : IHandleMessages<PublishCommandEvent>
    {
        static readonly ILog log = LogManager.GetLogger<PublishCommandEventHandler>();
        public async Task Handle(PublishCommandEvent message, IMessageHandlerContext context)
        {
            log.Info($"Event Message Receipt in CommandProject:{message.Id}");
        }
    }
}
