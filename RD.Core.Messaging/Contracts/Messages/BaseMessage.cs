using NServiceBus;
using RD.Core.Messaging.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Core.Messaging.Contracts.Messages
{
    public abstract class BaseMessage:Command,IMessage
    {
      
    }
}
