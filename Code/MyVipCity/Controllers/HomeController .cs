using System.Web.Mvc;

namespace MyVipCity.Controllers {

	[AllowAnonymous]
	public class HomeController: Controller {

		public ActionResult Index() {
			return View("Empty");
		}

		public ActionResult Home() {
			return View("Index");
		}
	}
}