using System.Web.Mvc;
using MyVipCity.Filters;

namespace MyVipCity {
	public class FilterConfig {
		public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
			filters.Add(new LogExceptionFilter());
			filters.Add(new HandleErrorAttribute());
			// filters.Add(new AuthorizeAttribute());
			filters.Add(new RequireHttpsAttribute());
		}
	}
}
