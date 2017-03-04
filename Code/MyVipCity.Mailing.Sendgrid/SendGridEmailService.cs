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

		public async Task SendBasicEmailAsync(BasicEmailModel model) {
			await Task.Run(async () => {
				Content content = new Content("text/html", model.Body);
				Email to = new Email(model.To);
				Email from = new Email(model.From);
				Mail mail = new Mail(from, model.Subject, to, content);
				await SendEmail(mail);
			});
		}

		public async Task SendConfirmationEmailAsync(ConfirmationEmailModel model) {
			await Task.Run(async () => {
				Content content = new Content("text/html", model.Body ?? "!");
				Email to = new Email(model.To);
				Email from = new Email(model.From);
				Mail mail = new Mail(from, model.Subject, to, content) { TemplateId = SendGridTemplateIds.ConfirmationEmailTemplateId };
				// add substitutions
				mail.Personalization[0].AddSubstitution("-confirmLink-", model.ConfirmationLink);
				await SendEmail(mail);
			});
		}

		public async Task SendForgotPasswordEmailAsync(ForgotPasswordEmailModel model) {
			await Task.Run(async () => {
				Content content = new Content("text/html", model.Body ?? "!");
				Email to = new Email(model.To);
				Email from = new Email(model.From);
				Mail mail = new Mail(from, model.Subject, to, content) { TemplateId = SendGridTemplateIds.ForgotPasswordTemplateId };
				// add substitutions
				mail.Personalization[0].AddSubstitution("-resetPasswordLink-", model.ResetPasswordLink);
				await SendEmail(mail);
			});
		}

		public async Task SendPromoterInvitationEmailAsync(PromoterInvitationEmailModel model) {
			await Task.Run(async () => {
				Content content = new Content("text/html", model.Body ?? "!");
				Email to = new Email(model.To);
				Email from = new Email(model.From);
				Mail mail = new Mail(from, model.Subject, to, content) { TemplateId = SendGridTemplateIds.PromoterInvitationTemplateId };
				// add substitutions
				mail.Personalization[0].AddSubstitution("-invitationUrl-", model.AcceptInvitationUrl);
				mail.Personalization[0].AddSubstitution("-name-", model.Name);
				mail.Personalization[0].AddSubstitution("-clubName-", model.ClubName);
				await SendEmail(mail);
			});
		}

		public async Task SendTestEmailAsync(TestEmailModel model) {
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
