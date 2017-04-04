using System;
using System.Threading.Tasks;
using System.Web.Http;
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
		public async Task<IHttpActionResult> AddBusiness(AttendingRequestDto attendingRequest) {
			attendingRequest.Email = User.Identity.Name;
			var acceptUrl = new Uri(Request.RequestUri, RequestContext.VirtualPathRoot) + "AttendingRequest?requestId={0}";
			await attendingRequestManager.SubmitRequestAsync(attendingRequest, acceptUrl);
			return Ok();
		}
	}
}