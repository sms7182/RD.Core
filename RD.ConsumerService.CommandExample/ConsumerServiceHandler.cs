using NServiceBus;
using NServiceBus.Logging;
using RDNameSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.ConsumerService.CommandExample
{
    public class ConsumerServiceHandler :
         IHandleMessages<ClientPublishCommand>
    {
        static readonly ILog log = LogManager.GetLogger<ConsumerServiceHandler>();
        static readonly Random random = new Random();
        public async Task Handle(ClientPublishCommand message, IMessageHandlerContext context)
        {
            log.Info($"Received ClientPublishCommand Id = {message.Id}");

            #region ThrowTransientException
            #endregion

            #region ThrowFatalException
            #endregion
            return ;
            //var orderPlaced = new OrderPlaced
            //{
            //    OrderId = message.Id.ToString()
            //};

            //log.Info($"Publishing OrderPlaced, OrderId = {message.Id}");

            //return context.Publish(orderPlaced);
        }
    }
}
