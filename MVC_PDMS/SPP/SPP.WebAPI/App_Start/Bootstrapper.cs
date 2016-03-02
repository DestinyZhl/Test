using Autofac;
using Autofac.Integration.WebApi;
using SPP.Core;
using SPP.Core.Authentication;
using SPP.Data.Infrastructure;
using SPP.Data.Repository;
using SPP.Service;
using SPP.WebAPI.Mappings;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace SPP.WebAPI
{
    public static class Bootstrapper
    {
        public static void Launch()
        {
            SetAutofacContainer();
            //Configure AutoMapper
            AutoMapperConfiguration.Configure();
        }

        private static void SetAutofacContainer()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>()
                   .As<IUnitOfWork>()
                   .InstancePerRequest();

            builder.RegisterType<DatabaseFactory>()
                   .As<IDatabaseFactory>()
                   .InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(SystemFunctionRepository).Assembly)
                   .Where(t => t.Name.EndsWith("Repository"))
                   .AsImplementedInterfaces()
                   .InstancePerRequest();

            builder.RegisterAssemblyTypes(typeof(SystemService).Assembly)
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces()
                   .InstancePerRequest();

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}