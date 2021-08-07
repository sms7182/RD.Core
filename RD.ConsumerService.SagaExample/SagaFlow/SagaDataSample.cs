using NServiceBus;
using RD.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.ConsumerService.SagaExample.SagaFlow
{
    public class SagaDataSample:SagaData
    {
        public Guid SagaIdentifier { get; set; }
    }
}
