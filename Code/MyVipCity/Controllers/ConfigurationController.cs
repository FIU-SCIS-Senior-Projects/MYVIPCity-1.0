using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ninject;
using Ninject.Extensions.Logging;

namespace MyVipCity.Controllers {

	public class ConfigurationController: VipControllerBase {

		[Inject]
		public ILogger Logger
		{
			get;
			set;
		}

		[Inject]
		public ApplicationUserManager ApplicationUserManager
		{
			get;
			set;
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult JsConfig() {
			// set configuration
			var config = new {
				Name = User.Identity.GetUserName(),
				Menu = GetNavigationMenu(),
				Routes = GetRoutes()
			};
			// set the JSON string
			ViewBag.ConfigObject = System.Web.Helpers.Json.Encode(config);
			// set response as javascript file
			Response.ContentType = "application/javascript";
			// return the view
			return View();
		}

		private object GetNavigationMenu() {
			var menu = new List<object> {
				new {
					Title = "Home",
					Path ="#/",
					Submenu = new object[] {
						new {
							Title = "After Login",
							Path = "#/Testing"
						},
						new {
							Title = "Example",
							Path = "Example"
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

			if (Request.IsAuthenticated) {
				// add menu item only if user is authenticated and in admin role
				if (IsUserInRole("Admin")) {
					menu.Add(new {
						Title = "Admin",
						Path = "/",
						Submenu = new object[] {
							new {
								Title = "Add Club",
								Path = "#/addbusiness"
							}
						}
					});
				}
			}
			return menu.ToArray();
		}

		private object GetRoutes() {
			var routes = new List<object> {
				new {
					Path = "/home",
					TemplateUrl = "/Home/Home"
				}
			};

			if (Request.IsAuthenticated) {
				// add route only if user is authenticated and in admin role
				if (IsUserInRole("Admin")) {
					routes.Add(new {
						Path = "/addbusiness",
						TemplateUrl = "/AddBusiness"
					});
				}
			}

			return routes.ToArray();
		}
	}
}
