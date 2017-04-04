using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MyVipCity.BusinessLogic;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;

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
	}
}