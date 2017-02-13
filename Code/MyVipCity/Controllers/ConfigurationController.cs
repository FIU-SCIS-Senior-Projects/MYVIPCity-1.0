using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MyVipCity.Controllers
{
	public class ConfigurationController : Controller {

		[HttpGet]
		[AllowAnonymous]
		public ActionResult JsConfig() {
			// set configuration
			var config = new {
				name = User.Identity.GetUserName()
			};
			// set the JSON string
			ViewBag.ConfigObject = System.Web.Helpers.Json.Encode(config);
			// set response as javascript file
			Response.ContentType = "application/javascript";
			// return the view
			return View();
		}
	}
}
