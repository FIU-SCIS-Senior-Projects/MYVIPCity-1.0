using System.Web.Mvc;

namespace MyVipCity.Controllers {

	[AllowAnonymous]
	public class HomeController: Controller {

		public ActionResult Index() {
			return View();
		}
	}
}