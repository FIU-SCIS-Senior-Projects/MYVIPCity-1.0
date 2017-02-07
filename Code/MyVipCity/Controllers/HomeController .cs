using System.Web.Mvc;
using MyVipCity.Mailing.Contracts;
using Ninject;

namespace MyVipCity.Controllers {

	[AllowAnonymous]
	public class HomeController: Controller {

		public ActionResult Index() {
			return View();
		}
	}
}