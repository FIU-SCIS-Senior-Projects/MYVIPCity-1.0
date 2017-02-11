using System.Threading.Tasks;
using MyVipCity.Mailing.Contracts.EmailModels;

namespace MyVipCity.Mailing.Contracts {

	public interface IEmailService {

		// Sends a regular email providing from, to, subject and body of the message
		Task SendBasicEmailAsync(BasicEmailModel model);

		// Sends a confirmation email to a just registered user
		Task SendConfirmationEmailAsync(ConfirmationEmailModel model);

		// Sends an email to reset a forgotten password.
		Task SendForgotPasswordEmailAsync(ForgotPasswordEmailModel model);

		// This sends a test email
		Task SendTestEmailAsync(TestEmailModel model);
	}
}