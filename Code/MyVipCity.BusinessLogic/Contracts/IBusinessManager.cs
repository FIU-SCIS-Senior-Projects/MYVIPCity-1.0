using MyVipCity.DataTransferObjects;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IBusinessManager {

		void Create(BusinessDto businessDto);

		BusinessDto Load(int id);
	}
}