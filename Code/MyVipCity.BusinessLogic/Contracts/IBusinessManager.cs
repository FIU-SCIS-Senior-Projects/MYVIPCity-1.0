using MyVipCity.DataTransferObjects;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IBusinessManager {

		BusinessDto Create(BusinessDto businessDto);

		BusinessDto LoadById(int id);

		BusinessDto LoadByFriendlyId(string friendlyId);
	}
}