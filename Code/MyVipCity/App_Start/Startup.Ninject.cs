using System.Configuration;
using System.Web.Http;
using MyVipCity.CompositionRoot;
using MyVipCity.Models;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi;
using Owin;

namespace MyVipCity {

	public partial class Startup {

		public void ConfigureNinject(IAppBuilder app) {
			app.UseNinjectMiddleware(CreateKernel);
		}

		private IKernel CreateKernel() {
			var kernel = new StandardKernel();
			try {
				// kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
				// kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

				RegisterServices(kernel);
				GlobalConfiguration.Configuration.DependencyResolver = new NinjectDependencyResolver(kernel);
				return kernel;
			}
			catch {
				kernel.Dispose();
				throw;
			}
		}

		private void RegisterServices(StandardKernel kernel) {
			kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
			BindingsManager.SetBindings(kernel, ConfigurationManager.AppSettings);
		}
	}
}