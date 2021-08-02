using RD.Core.Contract.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShareNameSpace
{
    public class ClientPublishCommand : BaseCommand
    {
     
        public ClientPublishCommand()
        {
            Id = Guid.NewGuid();
        }
    }
}
