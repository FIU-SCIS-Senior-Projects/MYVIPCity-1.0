using Microsoft.Owin;
using Ninject.Web.Common.OwinHost;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyVipCity.Startup))]
namespace MyVipCity
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) {
	        
			// configure authentication
			ConfigureAuth(app);
			// configure Ninject
	        ConfigureNinject(app);
        }
    }
}
