using System.Threading.Tasks;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IAttendingRequestManager {

		Task<ResultDto<bool>> SubmitRequestAsync(AttendingRequestDto attendingRequestDto, string acceptUrl);

		ResultDto<bool> SubmitRequest(AttendingRequestDto attendingRequestDto, string acceptUrl);

		Task<AttendingRequestDto> GetPendingRequestForPromoterAsync(int attendingRequestId, string promoterUserId);

		AttendingRequestDto GetPendingRequestForPromoter(int attendingRequestId, string promoterUserId);
	}
}