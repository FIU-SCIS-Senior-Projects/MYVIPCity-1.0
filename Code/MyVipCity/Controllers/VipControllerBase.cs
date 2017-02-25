using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace MyVipCity.Controllers {

	public abstract class VipControllerBase: Controller {

		protected string[] GetRolesFromUser() {
			if (!Request.IsAuthenticated)
				return null;
			var userIdentity = (ClaimsIdentity)User.Identity;
			var claims = userIdentity.Claims;
			var roleClaimType = userIdentity.RoleClaimType;
			var roles = claims.Where(c => c.Type == roleClaimType).Select(c => c.Value).ToArray();
			return roles;
		}

		protected bool IsUserInRole(string roleName) {
			if (string.IsNullOrWhiteSpace(roleName))
				throw new ArgumentNullException(nameof(roleName));
			var roles = GetRolesFromUser();
			return roles != null && roles.Contains(roleName);
		}
	}
}
