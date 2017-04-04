using System.Threading.Tasks;
using System.Web.Mvc;
using MyVipCity.BusinessLogic.Contracts;

namespace MyVipCity.Controllers {

	public class AttendingRequestController: VipControllerBase {

		private readonly IAttendingRequestManager attentingRequestManager;

		public AttendingRequestController(IAttendingRequestManager attentingRequestManager) {
			this.attentingRequestManager = attentingRequestManager;
			ViewBag.HideNgView = true;
		}


		// GET: AcceptPromoterInvitation
		public async Task<ActionResult> Index(int requestId) {
			// check if the user is not authenticated
			if (!Request.IsAuthenticated) {
				ViewBag.CustomMessage = "To see the details of an attending request you must log in first.";
				return View("NeedAuthentication");
			}

			// The user must be a promoter to accept or decline an attending request, otherwise redirect to home page
			if (!IsUserInRole("Promoter"))
				return Redirect("/");

			var attendingRequestDto = await attentingRequestManager.GetPendingRequestForPromoterAsync(requestId, UserId);

			// if a Pending Attending request does not exists for this promoter with the given id, then go to home page
			if (attendingRequestDto == null)
				return Redirect("/");

			return View(attendingRequestDto);
		}

		//[System.Web.Mvc.HttpPost]
		//[System.Web.Mvc.Authorize]
		//[ValidateAntiForgeryToken]
		//public ActionResult AcceptInvitation([FromBody]string friendlyId) {
		//	// accept invitation and create a new promoter profile
		//	PromoterProfileDto profileDto = PromoterInvitationManager.AcceptInvitation(friendlyId, UserEmail, UserId);
		//	// add user to Promoter role
		//	ApplicationUserManager.AddToRole(UserId, "Promoter");
		//	// sign out/and sign in the user again so that the new role is added to the claims identity
		//	var authManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
		//	authManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
		//	var user = ApplicationUserManager.FindById(UserId);
		//	var identity = ApplicationUserManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
		//	authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);


		//	return Redirect($"#/promoter-profile/{profileDto.Id}");
		//}
	}
}