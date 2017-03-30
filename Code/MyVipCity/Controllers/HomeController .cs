using System.Threading.Tasks;
using System.Web.Mvc;
using MyVipCity.IpGeolocation;
using MyVipCity.Utils.Extensions;

namespace MyVipCity.Controllers {

	[AllowAnonymous]
	public class HomeController: Controller {

		private readonly IIpGeolocator ipGeolocator;

		public HomeController(IIpGeolocator ipGeolocator) {
			this.ipGeolocator = ipGeolocator;
		}

		public ActionResult Index() {
			return View("Empty");
		}

		public async Task<ActionResult> Home() {
			var ip = Request.GetIpAddress();
			if (ip == "::1")
				ip = "73.205.175.112";
			var location = await ipGeolocator.LocateIpAddressAsync(ip);
			return View("Index", location);
		}
	}
}