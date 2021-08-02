using Microsoft.Extensions.Configuration;
using NServiceBus;
using RD.Core.Contract.Commands;
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

}
