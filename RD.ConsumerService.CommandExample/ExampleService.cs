using RD.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RD.ConsumerService.CommandExample
{
    public class ExampleService :BaseService, IExampleService
    {

        public ExampleService(IBus buse):base(buse)
        {
         

        }


    }
    public interface IExampleService:IBaseService
    {

    }
    
    public interface IBaseService
    {
        IBus Bus { get; }
    }
    public abstract  class BaseService:IBaseService
    {
        public BaseService(IBus bus)
        {
            Bus = bus;
        }
        public IBus Bus { get; set; }
    }

    

}
