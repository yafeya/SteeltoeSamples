﻿using Autofac;
using Autofac.Integration.Mvc;
using PostgreSql4.Models;
using Steeltoe.CloudFoundry.ConnectorAutofac;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PostgreSql4
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ServerConfig.RegisterConfig("development");

            var builder = new ContainerBuilder();

            // Register all the controllers with Autofac
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            builder.RegisterPostgreSqlConnection(ServerConfig.Configuration);

            // Create the Autofac container
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            // Initialize some data in PostgreSQL
            SampleData.InitializePostgreSqlData(container);
        }
    }
}
