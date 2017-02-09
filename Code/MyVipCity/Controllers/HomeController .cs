using System.Diagnostics;
using System.Web.Mvc;
using MyVipCity.Models;

namespace MyVipCity.Controllers {

	[AllowAnonymous]
	public class HomeController: Controller {

		public ActionResult Index() {
			var model = new HomeModel {
				Version = FileVersionInfo.GetVersionInfo(typeof(HomeController).Assembly.Location).FileVersion
			};
			return View(model);
		}
	}
}