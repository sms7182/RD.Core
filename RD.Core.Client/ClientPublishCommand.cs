using RD.Core.Contract.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDNameSpace
{
    public class ClientPublishCommand : ICommand
    {
        public Guid Id { get ; set ; }
        public ClientPublishCommand()
        {
            Id = Guid.NewGuid();
        }
    }
}
