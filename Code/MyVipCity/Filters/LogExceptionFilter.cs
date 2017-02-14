using System.Web.Mvc;
using Ninject.Extensions.Logging;

namespace MyVipCity.Filters {

	public class LogExceptionFilter: IExceptionFilter {

		/// <summary>
		/// Called when an exception occurs.
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public void OnException(ExceptionContext filterContext) {
			var logger = DependencyResolver.Current.GetService<ILogger>();
			logger?.Error(filterContext.Exception.Message + "\n" + filterContext.Exception.StackTrace);
		}
	}
}