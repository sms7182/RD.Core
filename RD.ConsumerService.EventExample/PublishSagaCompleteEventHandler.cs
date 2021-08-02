using NServiceBus.Logging;
using RD.ConsumerService.CommandExample;
using RD.ConsumerService.SagaExample.Events;
using RD.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.ConsumerService.EventExample
{
    public class PublishSagaCompleteEventHandler : BaseEventHandler<PublishSagaCompleteEvent>
    {
        static readonly ILog log = LogManager.GetLogger<PublishSagaCompleteEventHandler>();
        public override Task Handle(PublishSagaCompleteEvent @event)
        {
            log.Info($"Saga Complete with id:{@event.Id}");
            return Task.CompletedTask;
        }
    }
}
