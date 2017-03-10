using System.Security.Claims;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
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

		[Inject]
		public ApplicationUserManager ApplicationUserManager
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
			// find the invitation for this user for the given friendlyId
			var invitation = PromoterInvitationManager.GetPendingInvitation(friendlyId, UserEmail);
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

		[System.Web.Mvc.HttpPost]
		[System.Web.Mvc.Authorize]
		[ValidateAntiForgeryToken]
		public ActionResult AcceptInvitation([FromBody]string friendlyId) {
			// accept invitation and create a new promoter profile
			PromoterProfileDto profileDto = PromoterInvitationManager.AcceptInvitation(friendlyId, UserEmail, UserId);
			// add user to Promoter role
			ApplicationUserManager.AddToRole(UserId, "Promoter");
			return Redirect($"#/promoter-profile/{profileDto.Id}");
		}
	}
}