using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Controllers {

	public class AcceptPromoterInvitationController: VipControllerBase {

		public AcceptPromoterInvitationController(IPromoterInvitationManager promoterInvitationManager, IBusinessManager businessManager, ApplicationUserManager applicationUserManager) {
			PromoterInvitationManager = promoterInvitationManager;
			BusinessManager = businessManager;
			ApplicationUserManager = applicationUserManager;
			ViewBag.HideNgView = true;
		}

		public IPromoterInvitationManager PromoterInvitationManager
		{
			get;
			set;
		}

		public IBusinessManager BusinessManager
		{
			get;
			set;
		}

		public ApplicationUserManager ApplicationUserManager
		{
			get;
			set;
		}
		
		// GET: AcceptPromoterInvitation
		public ActionResult Index(string friendlyId) {
			// check if the user is not authenticated
			if (!Request.IsAuthenticated) {
				ViewBag.CustomMessage = "To accept an invitation to join a business you need to log in first. Please make sure you use the same em@il address where you received the invitation.";
				return View("NeedAuthentication");
			}
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
			// sign out/and sign in the user again so that the new role is added to the claims identity
			var authManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
			authManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
			var user = ApplicationUserManager.FindById(UserId);
			var identity = ApplicationUserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
			authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);


			return Redirect($"#/promoter-profile/{profileDto.Id}");
		}
	}
}