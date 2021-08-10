using NServiceBus;
using NServiceBus.Logging;
using RD.Core.Messaging;
using RD.ConsumerService.CommandExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShareNameSpace;

namespace RD.ConsumerService.CommandExample
{
    public class ConsumerServiceHandler : BaseCommandHandler<ClientPublishCommand>
   
    {
        static readonly ILog log = LogManager.GetLogger<ConsumerServiceHandler>();
        static readonly Random random = new Random();
        IExampleService _exampleService;
        public ConsumerServiceHandler(IExampleService exampleService)
        {
            _exampleService = exampleService;
          
        }

        public override Task Handle(ClientPublishCommand message)//, IMessageHandlerContext context)
        {
            log.Info($"Received ClientPublishCommand Id = {message.Id}");
            
             

            var publishCommmandEvent = new PublishCommandEvent
            {
                Id = message.Id
            };

            //log.Info($"Publishing OrderPlaced, OrderId = {message.Id}");

            //return context.Publish(orderPlaced);
            // this.Bus.Publish(publishCommmandEvent).Wait();
            return _exampleService.Bus.Publish(publishCommmandEvent);
            
        }

      
    }
}
