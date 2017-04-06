using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MyVipCity.IpGeolocation;
using MyVipCity.Utils;
using MyVipCity.Utils.Extensions;
using Newtonsoft.Json;

namespace MyVipCity.Controllers {

	[AllowAnonymous]
	public class HomeController: VipControllerBase {

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

			string ipLocationJsonString = GetCookie(MyVipCityCookies.IpLocation);

			IpLocation location;

			if (string.IsNullOrWhiteSpace(ipLocationJsonString)) {
				location = await FindIpLocationAndStoreItInCookieAsync(ip);
			}
			else {
				try {
					location = JsonConvert.DeserializeObject<IpLocation>(ipLocationJsonString);
					if (location.IpAddress != ip) {
						location = await FindIpLocationAndStoreItInCookieAsync(ip);
					}
				}
				catch (Exception) {
					location = await FindIpLocationAndStoreItInCookieAsync(ip);
				}
			}
			
			return View("Index", location);
		}

		private async Task<IpLocation> FindIpLocationAndStoreItInCookieAsync(string ip) {
			// find the geolocation by IP address
			var location = await ipGeolocator.LocateIpAddressAsync(ip);
			// serialize it into a JSON string
			var ipLocationJsonString = JsonConvert.SerializeObject(location);
			// save the json string as a cookie
			AddCookie(MyVipCityCookies.IpLocation, ipLocationJsonString);
			return location;
		}
	}
}