using System;
using System.Threading.Tasks;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Contracts.EmailModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MyVipCity.Mailing.Sendgrid {

	public class SendGridEmailService: IEmailService {

		private readonly SendGridAPIClient sendGridApiClient;

		public SendGridEmailService(string sendGridApiKey) {
			sendGridApiClient = new SendGridAPIClient(sendGridApiKey);
		}

		private async Task<dynamic> SendEmail(Mail mail) {
			dynamic response = await sendGridApiClient.client.mail.send.post(requestBody: mail.Get());
			return response;
		}

		public async Task SendTestEmail(TestEmailModel model) {
			await Task.Run(async () => {
				Content content = new Content("text/html", "!");
				Email to = new Email(model.To);
				Email from = new Email(model.From);
				Mail mail = new Mail(from, "Hello World - This is a test email", to, content) { TemplateId = SendGridTemplateIds.TestTemplateId };
				// add substitutions
				mail.Personalization[0].AddSubstitution("-name-", model.Name);
				await SendEmail(mail);
			});
		}
	}
}
