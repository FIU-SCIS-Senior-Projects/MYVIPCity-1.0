using MyVipCity.DataTransferObjects;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IPromoterInvitationManager {
		
		ResultDto<bool> SendPromoterInvitations(PromoterInvitationDto[] invitations, string baseUrl);

		PromoterInvitationDto[] GetPendingPromoterInvitations(string businessFriendlyId);

		void DeletePromoterInvitation(int id);

		PromoterInvitationDto GetPendingInvitation(string businessFriendlyId, string userEmail);

		PromoterProfileDto AcceptInvitation(string businessFriendlyId, string userEmail, string userId);
	}
}