using System.Collections.Generic;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IUserManager {

		string GetEmail(string userId);

		ICollection<string> GetAdminsEmail();
	}
}