using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Reflection;
using System.Web.Mvc;

namespace GyIMS
{
    public class AutofacConfig
    {
        public static void Register()
        {
            ContainerBuilder builder = new ContainerBuilder();


            Assembly assembly = Assembly.GetExecutingAssembly();
            builder.RegisterControllers(assembly);
            Type[] rtypes = assembly.GetTypes();
            builder.RegisterTypes(rtypes)
                .AsImplementedInterfaces();

            //Assembly servicesAss = Assembly.GetExecutingAssembly();// Assembly.Load("GyIMS");
            //Type[] stypes = servicesAss.GetTypes();
            //builder.RegisterTypes(stypes)
            //    .AsImplementedInterfaces();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}