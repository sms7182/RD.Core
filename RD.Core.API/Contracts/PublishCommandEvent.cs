using RD.Core.Contract.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD.ConsumerService.CommandExample
{
    public class PublishCommandEvent : BaseEvent
    {

        public PublishCommandEvent()
        {
            Id = Guid.NewGuid();
        }

    }
}
