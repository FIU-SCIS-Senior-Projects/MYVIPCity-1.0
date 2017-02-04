using System.Collections.Specialized;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Sendgrid;
using Ninject;

namespace MyVipCity.CompositionRoot {

	public static class BindingsManager {
		public static void SetBindings(IKernel kernel, NameValueCollection appSettings) {
			kernel.Bind<IEmailService>().To<SendGridEmailService>().WithConstructorArgument("sendGridApiKey", appSettings["myvipcity:send-grid-api"]);
		}
	}
}
