using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MyVipCity.Controllers {
	public class ConfigurationController: Controller {

		[HttpGet]
		[AllowAnonymous]
		public ActionResult JsConfig() {
			// set configuration
			var config = new {
				Name = User.Identity.GetUserName(),
				Menu = GetNavigationMenu()
			};
			// set the JSON string
			ViewBag.ConfigObject = System.Web.Helpers.Json.Encode(config);
			// set response as javascript file
			Response.ContentType = "application/javascript";
			// return the view
			return View();
		}

		private object GetNavigationMenu() {
			var menu = new object[] {
				new {
					Title = "Home",
					Path ="/",
					Submenu = new object[] {
						new {
							Title = "After Login",
							Path = "/"
						},
						new {
							Title = "Home Alternative",
							Path = "/"
						},
						new {
							Title = "Dashboard",
							Path = "/"
						}
					}
				},
				new {
					Title = "Listings",
					Path ="/",
					Submenu = new object[] {
						new {
							Title = "Grid view",
							Path = "/"
						},
						new {
							Title = "List view",
							Path = "/"
						},
						new {
							Title = "Map view",
							Path = "/"
						},
						new {
							Title = "Listing Detail",
							Path = "/"
						}
					}
				},
				new {
					Title = "Submit",
					Path ="/"
				},
				new {
					Title = "Clubs",
					Path ="/",
					Submenu = new object[] {
						new {
							Title = "Club Detail",
							Path = "/"
						},
						new {
							Title = "Club Reviews",
							Path = "/"
						},
						new {
							Title = "Club Disclaimer",
							Path = "/"
						}
					}
				},
				new {
					Title = "Promoters",
					Path ="/",
					Submenu = new object[] {
						new {
							Title = "Promoter Detail",
							Path = "/"
						},
						new {
							Title = "Promoter Reviews",
							Path = "/"
						}
					}
				}
			};
			return menu;
		}
	}
}
