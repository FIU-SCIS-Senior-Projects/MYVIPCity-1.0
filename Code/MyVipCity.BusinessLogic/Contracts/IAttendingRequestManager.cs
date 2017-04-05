using System.Threading.Tasks;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IAttendingRequestManager {

		Task<ResultDto<bool>> SubmitRequestAsync(AttendingRequestDto attendingRequestDto, string acceptUrl, string assignVipHostUrl);

		ResultDto<bool> SubmitRequest(AttendingRequestDto attendingRequestDto, string acceptUrl, string assignVipHostUrl);

		Task<AttendingRequestDto> GetPendingRequestForPromoterAsync(int attendingRequestId, string promoterUserId);

		AttendingRequestDto GetPendingRequestForPromoter(int attendingRequestId, string promoterUserId);

		Task<AttendingRequestDto> GetPendingRequestAsync(int attendingRequestId);

		AttendingRequestDto GetPendingRequest(int attendingRequestId);

		Task<ResultDto<bool>> AssignPromoterToRequestAsync(int attendingRequestId, int promoterId, string acceptUrl);

		ResultDto<bool> AssignPromoterToRequest(int attendingRequestId, int promoterId, string acceptUrl);

		Task<bool> AcceptRequestAsync(int attendingRequestId, string promoterUserId, string promoterProfileUrl);

		bool AcceptRequest(int attendingRequestId, string promoterUserId, string promoterProfileUrl);
	}
}