using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Core.Messaging
{
    public abstract class BaseEventHandler<T>: IHandleMessages<T> where T : RD.Core.Contract.Events.IEvent
    {

        IBus Bus { get; set; }
        public IMessageHandlerContext bus;
        public BaseEventHandler()
        {

        }


        public abstract Task Handle(T @event);

        public Task Handle(T message, IMessageHandlerContext context)
        {
            bus = context;

            Handle(message);
            return Task.CompletedTask;
        }
    }
}
