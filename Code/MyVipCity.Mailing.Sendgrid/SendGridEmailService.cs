using System.Linq;
using System.Threading.Tasks;
using MyVipCity.Mailing.Contracts;
using MyVipCity.Mailing.Contracts.EmailModels;
using MyVipCity.Mailing.Contracts.EmailModels.AttendingRequest;
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

		private void AddBccs(BasicEmailModel emailModel, Mail mail) {
			if (emailModel.Bccs == null)
				return;
			foreach (var bccEmail in emailModel.Bccs) {
				if (mail.Personalization[0].Tos.FirstOrDefault(email => email.Address == bccEmail) != null)
					continue;
				mail.Personalization[0].AddBcc(new Email(bccEmail));
			}
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
				Mail mail = GetMailFromBasicModel(model, SendGridTemplateIds.ConfirmationEmailTemplateId);
				// add substitutions
				mail.Personalization[0].AddSubstitution("-confirmLink-", model.ConfirmationLink);
				await SendEmail(mail);
			});
		}

		public async Task SendForgotPasswordEmailAsync(ForgotPasswordEmailModel model) {
			await Task.Run(async () => {
				Mail mail = GetMailFromBasicModel(model, SendGridTemplateIds.ForgotPasswordTemplateId);
				// add substitutions
				mail.Personalization[0].AddSubstitution("-resetPasswordLink-", model.ResetPasswordLink);
				await SendEmail(mail);
			});
		}

		public async Task SendPromoterInvitationEmailAsync(PromoterInvitationEmailModel model) {
			await Task.Run(async () => {
				Mail mail = GetMailFromBasicModel(model, SendGridTemplateIds.PromoterInvitationTemplateId);
				// add substitutions
				mail.Personalization[0].AddSubstitution("-invitationUrl-", model.AcceptInvitationUrl);
				mail.Personalization[0].AddSubstitution("-name-", model.Name);
				mail.Personalization[0].AddSubstitution("-clubName-", model.ClubName);
				await SendEmail(mail);
			});
		}

		public async Task SendPromoterReviewNotificationEmailAsync(PromoterReviewNotificationEmailModel model) {
			await Task.Run(async () => {
				Mail mail = GetMailFromBasicModel(model, SendGridTemplateIds.PromoterReviewNotificationTemplateId);
				// add substitutions
				mail.Personalization[0].AddSubstitution("-businessName-", model.BusinessName);
				mail.Personalization[0].AddSubstitution("-rating-", model.Rating);
				mail.Personalization[0].AddSubstitution("-comment-", model.Comment);
				await SendEmail(mail);
			});
		}

		public async Task SendAttendigRequestNotificationToPromoterAsync(NewAttendingRequestPromoterNotificationEmailModel model) {
			await Task.Run(async () => {
				Mail mail = GetMailFromBasicModel(model, SendGridTemplateIds.NewAttendingRequestNotificationForPromoterTemplateId);
				// add substitutions
				mail.Personalization[0].AddSubstitution("-promoterName-", model.PromoterName);
				mail.Personalization[0].AddSubstitution("-acceptLink-", model.AcceptLink);
				mail.Personalization[0].AddSubstitution("-declineLink-", model.DeclineLink);

				AddSubstitutionsForNewAttendingRequestEmail(model, mail);

				AddBccs(model, mail);

				await SendEmail(mail);
			});
		}

		public async Task SendAttendigRequestNotificationToAdminAsync(NewAttendingRequestAdminNotificationEmailModel model) {
			await Task.Run(async () => {
				Mail mail = GetMailFromBasicModel(model, SendGridTemplateIds.NewAttendingRequestWithoutPromoterNotificationTemplateId);
				// add substitutions
				mail.Personalization[0].AddSubstitution("-adminName-", model.AdminName);
				mail.Personalization[0].AddSubstitution("-assignVipHostUrl-", model.AssignVipHostUrl);

				AddSubstitutionsForNewAttendingRequestEmail(model, mail);

				AddBccs(model, mail);

				await SendEmail(mail);
			});
		}

		private void AddSubstitutionsForNewAttendingRequestEmail(NewAttendingRequestNotificationEmailModel model, Mail mail) {
			mail.Personalization[0].AddSubstitution("-businessName-", model.BusinessName);
			mail.Personalization[0].AddSubstitution("-name-", model.Name);
			mail.Personalization[0].AddSubstitution("-email-", model.Email);
			mail.Personalization[0].AddSubstitution("-phone-", model.Phone);
			mail.Personalization[0].AddSubstitution("-partyCount-", model.PartyCount.ToString());
			mail.Personalization[0].AddSubstitution("-femaleCount-", model.FemaleCount.ToString());
			mail.Personalization[0].AddSubstitution("-maleCount-", model.MaleCount.ToString());
			mail.Personalization[0].AddSubstitution("-date-", model.Date);
			mail.Personalization[0].AddSubstitution("-message-", model.Message);
			mail.Personalization[0].AddSubstitution("-service-", model.Service);
		}

		public async Task SendAcceptedAttendingRequestNotificationToUserAsync(AcceptedAttendingRequestNotificationEmailModel model) {
			await Task.Run(async () => {
				Mail mail = GetMailFromBasicModel(model, SendGridTemplateIds.AcceptedAttendingRequestNotificationTemplateId);
				// add substitutions
				mail.Personalization[0].AddSubstitution("-name-", model.Name);
				mail.Personalization[0].AddSubstitution("-businessName-", model.BusinessName);
				mail.Personalization[0].AddSubstitution("-date-", model.Date);
				mail.Personalization[0].AddSubstitution("-partyCount-", model.PartyCount.ToString());
				mail.Personalization[0].AddSubstitution("-vipHost-", model.VipHostName);
				mail.Personalization[0].AddSubstitution("-vipHostPageLink-", model.VipHostPageLink);

				AddBccs(model, mail);

				await SendEmail(mail);
			});
		}

		public async Task SendDeclinedAttendingRequestNotificationToAdminAsync(DeclinedAttendingRequestAdminNotificationEmailModel model) {
			Mail mail = GetMailFromBasicModel(model, SendGridTemplateIds.AttendingRequestDeclinedNotificationToAdminTemplateId);
			// add substitutions
			mail.Personalization[0].AddSubstitution("-adminName-", model.AdminName);
			mail.Personalization[0].AddSubstitution("-assignVipHostUrl-", model.AssignVipHostUrl);
			mail.Personalization[0].AddSubstitution("-vipHostName-", model.VipHostName);
			AddSubstitutionsForNewAttendingRequestEmail(model, mail);
			AddBccs(model, mail);
			await SendEmail(mail);
		}

		private Mail GetMailFromBasicModel(BasicEmailModel model, string templateId) {
			Content content = new Content("text/html", model.Body ?? "!");
			Email to = new Email(model.To);
			Email from = new Email(model.From);
			Mail mail = new Mail(from, model.Subject, to, content) { TemplateId = templateId };
			return mail;
		}
	}
}
