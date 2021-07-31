using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Core.Contract.Messages
{
    public interface IMessage:NServiceBus.IMessage
    {
        Guid Id { get; set; }
    }
}
