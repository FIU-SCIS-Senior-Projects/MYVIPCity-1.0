using System.Collections.Specialized;
using System.Linq;
using AutoMapper;
using AutoMapper.Mappers;
using MyVipCity.BusinessLogic;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.Domain.Automapper;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Sendgrid;
using Ninject;
using Ninject.Web.Common;

namespace MyVipCity.CompositionRoot {

	public static class BindingsManager {

		public static void SetBindings(IKernel kernel, NameValueCollection appSettings) {
			// bind IEmailService -> SendGridEmailService
			kernel.Bind<IEmailService>().To<SendGridEmailService>().InSingletonScope().WithConstructorArgument("sendGridApiKey", appSettings["myvipcity:send-grid-api"]);

			// bind IMapper
			kernel.Bind<IMapper>()
				.ToMethod(context => {
					var config = new MapperConfiguration(cfg => {
						// load assembly with automapper profiles
						var autoMapperProfilesAssembly = typeof(BusinessProfile).Assembly;
						// register the profiles
						cfg.AddProfiles(autoMapperProfilesAssembly);
						// register profile defined in Automapper.Collection
						cfg.AddProfile<CollectionProfile>();
					});
					// create mapper
					var mapper = config.CreateMapper();
					return mapper;
				})
				.InSingletonScope();

			BusinessLayerBindings(kernel);
		}

		private static void BusinessLayerBindings(IKernel kernel) {
			kernel.Bind<IBusinessManager>().To<BusinessManager>().InRequestScope();
			kernel.Bind<IPromoterInvitationManager>().To<PromoterInvitationManager>().InRequestScope();
			kernel.Bind<IPromoterProfileManager>().To<PromoterProfileManager>().InRequestScope();
		}
	}
}
