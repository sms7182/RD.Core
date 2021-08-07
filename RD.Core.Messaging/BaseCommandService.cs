using Microsoft.Extensions.Configuration;
using NServiceBus;
using RD.Core.Messaging.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Core.Messaging
{
    public abstract class BaseCommandHandler<T>: IHandleMessages<T> where T:BaseCommand
    {
        IBus Bus { get; set; }
        public IMessageHandlerContext bus;
        public BaseCommandHandler()
        {

        }
          

        public abstract  Task Handle(T command);

        public Task Handle(T message, IMessageHandlerContext context)
        {
            bus = context;
            
            Handle(message);
            return Task.CompletedTask;
        }
    }

    public abstract class BaseSagaHandler<T, U> : Saga<T>,IAmStartedByMessages<U> where T:ContainSagaData,new() where U:BaseCommand
    {
       

        public abstract Task Handle(U command);
        public Task Handle(U message,IMessageHandlerContext context)
        {
            Handle(message);
            return Task.CompletedTask;
        }


        public abstract void ConfigureSagaFinder(SagaPropertyMapper<T> mapper);
        protected override void ConfigureHowToFindSaga(IConfigureHowToFindSagaWithMessage sagaMessageFindingConfiguration)
        {
            base.ConfigureHowToFindSaga(sagaMessageFindingConfiguration);
        }

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<T> mapper)
        {
            
            ConfigureSagaFinder(mapper);
        }
    }

    public class SagaData : ContainSagaData
    {
        public SagaData()
        {
            CreationDate = DateTime.Now;
        }
        public DateTime CreationDate { get; set; }
    }


}
