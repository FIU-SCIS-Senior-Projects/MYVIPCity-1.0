using System.Threading.Tasks;
using System.Web.Http;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Controllers.api {

	[RoutePrefix("api/AttendingRequest")]
	public class AttendingRequestController: ApiController {

		private IAttentingRequestManager attentingRequestManager;

		public AttendingRequestController(IAttentingRequestManager attentingRequestManager) {
			this.attentingRequestManager = attentingRequestManager;
		}

		[HttpPost]
		[Authorize]
		[Route("")]
		public async Task<IHttpActionResult> AddBusiness(AttendingRequestDto attendingRequest) {
			attendingRequest.Email = User.Identity.Name;
			await attentingRequestManager.SubmitRequestAsync(attendingRequest);
			return Ok();
		}
	}
}