using System.Collections.Specialized;
using AutoMapper;
using AutoMapper.Mappers;
using MyVipCity.BusinessLogic;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.Common;
using MyVipCity.Domain.Automapper;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Sendgrid;
using Ninject;
using Ninject.Extensions.Logging;
using Ninject.Extensions.Logging.Log4net.Infrastructure;
using Container = SimpleInjector.Container;

namespace MyVipCity.CompositionRoot {

	public static class BindingsManager {

		public static void RegisterServices(Container container, NameValueCollection appSettings) {
			container.RegisterSingleton<IResolver>(new SimpleInjectorResolver(container));
			container.RegisterSingleton<IEmailService>(new SendGridEmailService(appSettings["myvipcity:send-grid-api"]));
			container.RegisterSingleton<IMapper>(() => {
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
			});
			// register log4net
			container.RegisterSingleton<ILoggerFactory, Log4NetLoggerFactory>();
			container.RegisterSingleton<ILogger>(() => container.GetInstance<ILoggerFactory>().GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType));
			BusinessLayerServices(container);
		}

		public static void SetBindings(IKernel kernel, NameValueCollection appSettings) {
			// bind IResolver
			kernel.Bind<IResolver>().To<NinjectResolver>().InSingletonScope().WithConstructorArgument("kernel", kernel);

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

		private static void BusinessLayerServices(Container container) {
			container.RegisterSingleton<IPostsEntityManager, PostsEntityManager>();
			container.RegisterSingleton<IBusinessManager, BusinessManager>();
			container.RegisterSingleton<IPromoterInvitationManager, PromoterInvitationManager>();
			container.RegisterSingleton<IPromoterProfileManager, PromoterProfileManager>();
		}

		private static void BusinessLayerBindings(IKernel kernel) {
			kernel.Bind<IPostsEntityManager>().To<PostsEntityManager>().InSingletonScope();
			kernel.Bind<IBusinessManager>().To<BusinessManager>().InSingletonScope();
			kernel.Bind<IPromoterInvitationManager>().To<PromoterInvitationManager>().InSingletonScope();
			kernel.Bind<IPromoterProfileManager>().To<PromoterProfileManager>().InSingletonScope();
		}
	}
}
