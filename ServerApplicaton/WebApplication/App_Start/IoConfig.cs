using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Repositories.Implementations;
using Repositories.Interfaces;

namespace WebApplication
{
    public class IoConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            var domain = AppDomain.CurrentDomain;
            var currentPath = Path.Combine(domain.BaseDirectory, "bin");
            //Load all dlls from bin folder in every cases
            PreLoad(currentPath);
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof (MvcApplication).Assembly);
            builder.RegisterType<Database>().As<IDatabase>().SingleInstance().InstancePerHttpRequest();


            builder.Register(c => new HttpContextWrapper(HttpContext.Current))
                .As<HttpContextBase>()
                .InstancePerHttpRequest();

            var assemblys = domain.GetAssemblies();

            //Register all repositories
            builder.RegisterType<CommonRepository>()
                .As<ICommonRepository>()
                .InstancePerDependency()
                .InstancePerHttpRequest();

            //Register all services
            var services = assemblys.FirstOrDefault(c => c.FullName.StartsWith("Services"));


            builder.RegisterAssemblyTypes(services)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerHttpRequest();

            //We build the container.
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static void PreLoad(string p)
        {
            //all try/catch blocks are elided for brevity
            string[] files = Directory.GetFiles(p, "*.dll", SearchOption.AllDirectories);
            foreach (var s in files)
            {
                var assemblyName = AssemblyName.GetAssemblyName(s);
                var assemblys = AppDomain.CurrentDomain.GetAssemblies();
                if (
                    !assemblys.Any(assembly => AssemblyName.ReferenceMatchesDefinition(assembly.GetName(), assemblyName)))
                    Assembly.LoadFrom(s);
            }
        }
    }
}