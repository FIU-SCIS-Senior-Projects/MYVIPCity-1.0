using System.Threading.Tasks;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IAttentingRequestManager {

		Task<ResultDto<bool>> SubmitRequestAsync(AttendingRequestDto attendingRequestDto);

		ResultDto<bool> SubmitRequest(AttendingRequestDto attendingRequestDto);
	}
}