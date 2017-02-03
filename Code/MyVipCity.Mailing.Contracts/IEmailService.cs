using System.Threading.Tasks;
using MyVipCity.Mailing.Contracts.EmailModels;

namespace MyVipCity.Mailing.Contracts {

	public interface IEmailService {

		// This sends a test email
		Task SendTestEmail(TestEmailModel model);
	}
}