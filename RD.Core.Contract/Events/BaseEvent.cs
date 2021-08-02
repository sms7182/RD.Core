using NServiceBus;
using RD.Core.Contract.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Core.Contract.Events
{
    public abstract class BaseEvent:Command, IEvent
    {
        
    }
}
