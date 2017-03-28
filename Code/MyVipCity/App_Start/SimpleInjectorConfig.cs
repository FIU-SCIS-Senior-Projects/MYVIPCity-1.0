using System.Configuration;
using System.Data.Entity;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using MyVipCity.CompositionRoot;
using MyVipCity.Models;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Integration.WebApi;

namespace MyVipCity {

	public static class SimpleInjectorConfig {

		public static void RegisterContainer(HttpApplication httpApplication) {
			// create the IoC container
			var container = new Container();
			// set the scoped lifestyle to WebRequest
			// container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
			var webApiRequestLifeStyle = new WebApiRequestLifestyle();
			container.Options.DefaultScopedLifestyle = Lifestyle.CreateHybrid(
					lifestyleSelector: () => webApiRequestLifeStyle.GetCurrentScope(container) != null,
					trueLifestyle: webApiRequestLifeStyle,
					falseLifestyle: new WebRequestLifestyle()
				);

			// register services
			RegisterServices(container, httpApplication);
			/* The Verify method provides a fail-fast mechanism to prevent your application 
			 * from starting when the Container has been accidentally misconfigured. The Verify 
			 * method checks the entire configuration by creating an instance of each registered type.
			 http://simpleinjector.readthedocs.io/en/latest/using.html#verifying-the-container-s-configuration */
			// container.Verify();

			// set dependency resolver for MVC
			DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
			// set dependency resolver for WebApi
			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
		}

		private static void RegisterServices(Container container, HttpApplication httpApplication) {
			// register services from this assembly
			container.Register<ApplicationDbContext>(ApplicationDbContext.Create, Lifestyle.Scoped);
			container.Register<DbContext, ApplicationDbContext>(Lifestyle.Scoped);
			container.Register<ApplicationUserManager>(() => {
				return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
			}, Lifestyle.Scoped);
			container.Register<ApplicationSignInManager>(() => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>(), Lifestyle.Scoped);
			// register WebApi controller
			container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
			// register MVC controller
			container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
			// register services in CompositionRoot
			BindingsManager.RegisterServices(container, ConfigurationManager.AppSettings);
		}
	}
}