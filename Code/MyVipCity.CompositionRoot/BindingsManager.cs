using System.Collections.Specialized;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Sendgrid;
using Ninject;

namespace MyVipCity.CompositionRoot {

	public static class BindingsManager {
		public static void SetBindings(IKernel kernel, NameValueCollection appSettings) {
			// TODO: Set actual sendgrid api key
			kernel.Bind<IEmailService>().To<SendGridEmailService>().WithConstructorArgument("sendGridApiKey", "api");
		}
	}
}
