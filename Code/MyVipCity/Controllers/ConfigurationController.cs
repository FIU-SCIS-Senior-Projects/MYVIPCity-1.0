using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyVipCity.BusinessLogic.Contracts;

namespace MyVipCity.Controllers {

	public class ConfigurationController: VipControllerBase {

		public ConfigurationController(IPromoterProfileManager promoterProfileManager) {
			PromoterProfileManager = promoterProfileManager;
		}

		public IPromoterProfileManager PromoterProfileManager
		{
			get;
			set;
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult JsConfig() {
			var roles = GetRolesFromUser();
			// set configuration
			dynamic config = new ExpandoObject();

			config.IsAuthenticated = Request.IsAuthenticated;
			if (config.IsAuthenticated) {
				config.Name = User.Identity.GetUserName();
				config.Roles = roles;
			}
			try {
				config.Menu = GetNavigationMenu();
				config.Routes = GetRoutes();
			}
			catch (Exception e) {
				var x = 1;
			}

			if (roles != null && roles.Contains("Promoter")) {
				var promoterProfiles = PromoterProfileManager.GetPromoterProfiles(UserId);
				config.PromoterProfileIds = promoterProfiles.Select(pp => pp.Id).ToArray();
			}

			// set the JSON string
			ViewBag.ConfigObject = Newtonsoft.Json.JsonConvert.SerializeObject(config); // System.Web.Helpers.Json.Encode(config);
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
					Title = "Restaurants",
					Path ="/",
					Submenu = new object[] {
						new {
							Title = "Restaurant Detail",
							Path = "/"
						},
						new {
							Title = "Restaurant Reviews",
							Path = "/"
						},
						new {
							Title = "Restaurant Disclaimer",
							Path = "/"
						}
					}
				}
			};

			if (Request.IsAuthenticated) {
				if (IsUserInRole("Promoter")) {
					// get all promoter profiles
					var promoterProfiles = PromoterProfileManager.GetPromoterProfiles(UserId);
					// create a submenu for each profile
					var promoterProfileSubmenu = promoterProfiles.OrderBy(p => p.Business.Name).Select(p => new {
						Title = p.Business.Name,
						Path = $"#/promoter-profile/{p.Id}"
					}).ToArray();
					// add a menu entry if there is at least one promoter profile
					if (promoterProfileSubmenu.Any()) {
						menu.Add(new {
							Title = "My Promoter Profile",
							Path = "/",
							Submenu = promoterProfileSubmenu
						});
					}
				}

				// add menu item only if user is authenticated and in admin role
				if (IsUserInRole("Admin")) {
					menu.Add(new {
						Title = "Admin",
						Path = "/",
						Submenu = new object[] {
							new {
								Title = "Add Restaurant",
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
				},
				new {
					Path = "/view-business/:friendlyId",
					TemplateUrl = "/ViewBusiness",
					Controller = "vip.viewBusinessController"
				},
				new {
					Path = "/promoter-profile/:promoterProfileId",
					TemplateUrl = "/PromoterProfile",
					Controller = "vip.promoterProfileController"
				}
			};

			if (Request.IsAuthenticated) {
				// add route only if user is authenticated and in admin role
				if (IsUserInRole("Admin")) {
					routes.Add(new {
						Path = "/addbusiness",
						TemplateUrl = "/AddBusiness",
						Controller = "vip.addBusinessController"
					});
				}
			}

			return routes.ToArray();
		}
	}
}
