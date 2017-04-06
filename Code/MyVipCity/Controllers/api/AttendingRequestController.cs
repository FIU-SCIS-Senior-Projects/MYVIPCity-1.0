using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Controllers.api {

	[RoutePrefix("api/AttendingRequest")]
	public class AttendingRequestController: ApiController {

		private readonly IAttendingRequestManager attendingRequestManager;

		public AttendingRequestController(IAttendingRequestManager attendingRequestManager) {
			this.attendingRequestManager = attendingRequestManager;
		}

		[HttpPost]
		[Authorize]
		[Route("")]
		public async Task<IHttpActionResult> SubmitAttendingRequest(AttendingRequestDto attendingRequest) {
			attendingRequest.Email = User.Identity.Name;
			var acceptUrl = new Uri(Request.RequestUri, RequestContext.VirtualPathRoot) + "AttendingRequest?requestId={0}";
			var assignVipHostUrl = new Uri(Request.RequestUri, RequestContext.VirtualPathRoot) + "AttendingRequest/AssignPromoter?requestId={0}";
			var result = await attendingRequestManager.SubmitRequestAsync(attendingRequest, acceptUrl, assignVipHostUrl);
			return Ok(result);
		}

		[HttpPost]
		[Authorize(Roles = "Promoter")]
		[Route("Accept/{requestId:int}")]
		public async Task<IHttpActionResult> Accept(int requestId) {
			var promoterUrl = new Uri(Request.RequestUri, RequestContext.VirtualPathRoot) + "#/promoter-profile/{0}";
			var result = await attendingRequestManager.AcceptRequestAsync(requestId, User.Identity.GetUserId(), promoterUrl);
			return Ok(result);
		}


		[HttpPost]
		[Authorize(Roles = "Promoter")]
		[Route("DeclineByPromoter/{requestId:int}")]
		public async Task<IHttpActionResult> DeclineByPromoter(int requestId) {
			var assignVipHostUrl = new Uri(Request.RequestUri, RequestContext.VirtualPathRoot) + "AttendingRequest/AssignPromoter?requestId={0}";
			var result = await attendingRequestManager.DeclineByPromoterAsync(requestId, User.Identity.GetUserId(), assignVipHostUrl);
			return Ok(result);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("DeclineByAdmin/{requestId:int}")]
		public async Task<IHttpActionResult> DeclineByAdmin(int requestId) {
			var result = await attendingRequestManager.DeclineByAdminAsync(requestId);
			return Ok(result);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("AssignPromoter/{requestId:int}/{promoterId:int}")]
		public async Task<IHttpActionResult> AssignPromoter(int requestId, int promoterId) {
			var acceptUrl = new Uri(Request.RequestUri, RequestContext.VirtualPathRoot) + "AttendingRequest?requestId={0}";
			var result = await attendingRequestManager.AssignPromoterToRequestAsync(requestId, promoterId, acceptUrl);
			return Ok(result);
		}
	}
}