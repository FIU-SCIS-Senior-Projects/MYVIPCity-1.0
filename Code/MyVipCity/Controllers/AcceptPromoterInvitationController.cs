using System.Security.Claims;
using System.Web.Mvc;
using MyVipCity.BusinessLogic.Contracts;
using Ninject;

namespace MyVipCity.Controllers {

	public class AcceptPromoterInvitationController: VipControllerBase {

		[Inject]
		public IPromoterInvitationManager PromoterInvitationManager
		{
			get;
			set;
		}

		[Inject]
		public IBusinessManager BusinessManager
		{
			get;
			set;
		}

		public AcceptPromoterInvitationController() {
			ViewBag.HideNgView = true;
		}

		// GET: AcceptPromoterInvitation
		public ActionResult Index(string friendlyId) {
			// check if the user is not authenticated
			if (!Request.IsAuthenticated)
				return View("NeedAuthentication");
			// at this point, we know the user is authenticated
			var userIdentity = (ClaimsIdentity)User.Identity;
			// find the invitation for this user for the given friendlyId
			var invitation = PromoterInvitationManager.GetPendingInvitation(friendlyId, userIdentity.Name);
			// if there is no pending invitation, then redirect to home page
			if (invitation == null)
				return Redirect("/");
			// get the business
			var businessDto = BusinessManager.LoadByFriendlyId(friendlyId);
			// if the business was not found, then redirect to home page
			if (businessDto == null)
				return Redirect("/");

			return View(businessDto);
		}
	}
}