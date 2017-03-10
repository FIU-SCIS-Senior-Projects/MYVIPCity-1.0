using MyVipCity.DataTransferObjects;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IPromoterProfileManager {

		PromoterProfileDto[] GetPromoterProfiles(string userId);

		PromoterProfileDto GetProfileById(int id);

		PromoterProfileDto Update(PromoterProfileDto promoterProfileDto);

		string GetPromoterEmail(int id);
	}
}