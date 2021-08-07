using NServiceBus;
using RD.Core.Messaging.Contracts.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Core.Messaging.Contracts.Events
{
    public abstract class BaseEvent:Command, IEvent
    {
        
    }
}
