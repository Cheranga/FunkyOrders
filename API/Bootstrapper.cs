using API.Util;
using Autofac;
using AzureFunctions.Autofac.Configuration;

namespace API
{
    public class Bootstrapper
    {
        public Bootstrapper(string functionName)
        {
            DependencyInjection.Initialize(builder => { builder.RegisterType<OrderProcessor>().As<IOrderProcessor>(); }, functionName);
        }
    }
}