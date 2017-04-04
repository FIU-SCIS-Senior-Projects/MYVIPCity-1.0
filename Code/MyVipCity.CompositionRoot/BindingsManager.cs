using System.Collections.Specialized;
using AutoMapper;
using AutoMapper.Mappers;
using MyVipCity.BusinessLogic;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.Common;
using MyVipCity.Domain.Automapper;
using MyVipCity.IpGeolocation;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Sendgrid;
using Ninject.Extensions.Logging;
using Ninject.Extensions.Logging.Log4net.Infrastructure;
using Container = SimpleInjector.Container;

namespace MyVipCity.CompositionRoot {

	public static class BindingsManager {

		public static void RegisterServices(Container container, NameValueCollection appSettings) {
			container.RegisterSingleton<IIpGeolocator>(new IpInfoDbIpGeoLocator(appSettings["myvipcity:ip-info-db-api-key"]));
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

		private static void BusinessLayerServices(Container container) {
			container.RegisterSingleton<IUserManager, UserManager>();
			container.RegisterSingleton<IAttendingRequestManager, AttendingRequestManager>();
			container.RegisterSingleton<IPostsEntityManager, PostsEntityManager>();
			container.RegisterSingleton<IBusinessManager, BusinessManager>();
			container.RegisterSingleton<IPromoterInvitationManager, PromoterInvitationManager>();
			container.RegisterSingleton<IPromoterProfileManager, PromoterProfileManager>();
		}
	}
}
