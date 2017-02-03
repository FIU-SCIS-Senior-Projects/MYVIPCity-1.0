using System.Web.Mvc;

namespace MyVipCity.Controllers {

	[AllowAnonymous]
	public class HomeController: Controller {

		public ActionResult Index() {
			return View();
		}

		public ActionResult About() {
			ViewBag.Message = "Welcome too MyVIPCity";

			return View();
		}

		public ActionResult Contact() {
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}