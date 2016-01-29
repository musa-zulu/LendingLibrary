using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using LendingLibrary.Web.Repository;

namespace LendingLibrary.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Boostrap();
        }

        private IWindsorContainer Boostrap()
        {
            var container = new WindsorContainer();
            container.Install(FromAssembly.This());
            container.Register(Component.For<IWindsorContainer>()
                .Instance(container));
            
            container.Register(
               Component
                   .For<IControllerFactory>()
                   .ImplementedBy<WindsorControllerFactory>()
                   .LifeStyle.Singleton
               );

            container.Register(Classes.FromThisAssembly()
              .BasedOn<IController>()
              .LifestyleTransient());
            
            container.Register(Component.For<IItemsRepository>().ImplementedBy<ItemsRepository>());
            SetControllerFactory(container);

            return container;
        }

        private static void SetControllerFactory(WindsorContainer container)
        {
            var windsorControllerFactory = container.Resolve<IControllerFactory>();
            ControllerBuilder.Current.SetControllerFactory(windsorControllerFactory);
        }
    }
}
