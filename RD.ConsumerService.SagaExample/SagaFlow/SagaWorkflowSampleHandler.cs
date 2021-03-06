using NServiceBus;
using RD.ConsumerService.SagaExample.Events;
using RD.ConsumerService.SagaExample.SagaFlow;
using RD.Core.Messaging;
using SagaNameSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.ConsumerService.SagaExample
{
    public class SagaWorkflowSampleHandler :BaseSagaHandler<SagaDataSample,SagaStartingCommand> ,IHandleMessages<SagaCompleteCommand>
        
    {
        
    
        public Task Handle(SagaCompleteCommand message, IMessageHandlerContext context)
        {
            MarkAsComplete();
            var eventCommand= new PublishSagaCompleteEvent();
            eventCommand.Id = message.Id;
            context.Publish(eventCommand).Wait();
            return Task.CompletedTask;
        }

        public override Task Handle(SagaStartingCommand command)
        {
            return Task.CompletedTask;
        }

        public override void ConfigureSagaFinder(SagaPropertyMapper<SagaDataSample> mapper)
        {
            mapper.ConfigureMapping<SagaStartingCommand>(message => message.Id).ToSaga(sagaData => sagaData.SagaIdentifier);
            mapper.ConfigureMapping<SagaCompleteCommand>(message => message.Id).ToSaga(sagaData => sagaData.SagaIdentifier);
        }
    }
}
