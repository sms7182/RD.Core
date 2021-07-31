using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Core.Contract.Events
{
    public interface IEvent: NServiceBus.IEvent
    {
        Guid Id { get; set; }
    }
}
