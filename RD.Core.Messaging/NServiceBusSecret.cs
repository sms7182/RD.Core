using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RD.Core.Messaging
{
    public class NServiceBusSecret
    {
        public string EndpointHost { get; set; }
        public string DestinationEndpointHost { get; set; }
        public string RabbitConnectionInfo { get; set; }
        public string SagaEndpointHost { get; set; }
    }
}
