using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.Core.Contract.Commands
{
    public abstract class BaseCommand:Command,ICommand
    {
    }
}
