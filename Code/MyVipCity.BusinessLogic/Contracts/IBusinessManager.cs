using MyVipCity.DataTransferObjects;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IBusinessManager {

		BusinessDto Create(BusinessDto businessDto);

		BusinessDto Update(BusinessDto businessDto);

		BusinessDto LoadById(int id);

		BusinessDto LoadByFriendlyId(string friendlyId);

		BusinessDto[] GetAllBusiness();

		bool SendPromoterInvitations(PromoterInvitationDto[] invitations, string baseUrl);
	}
}