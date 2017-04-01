﻿using System.Linq;
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

		public async Task SendPromoterReviewNotificationEmailAsync(PromoterReviewNotificationEmailModel model) {
			await Task.Run(async () => {
				Content content = new Content("text/html", model.Body ?? "!");
				Email to = new Email(model.To);
				Email from = new Email(model.From);
				Mail mail = new Mail(from, model.Subject, to, content) { TemplateId = SendGridTemplateIds.PromoterReviewNotificationTemplateId };
				// add substitutions
				mail.Personalization[0].AddSubstitution("-businessName-", model.BusinessName);
				mail.Personalization[0].AddSubstitution("-rating-", model.Rating);
				mail.Personalization[0].AddSubstitution("-comment-", model.Comment);
				await SendEmail(mail);
			});
		}

		public async Task SendAttendigRequestNotificationToPromoter(NewAttendingRequestPromoterNotificationEmailModel model) {
			await Task.Run(async () => {
				Content content = new Content("text/html", model.Body ?? "!");
				Email to = new Email(model.To);
				Email from = new Email(model.From);
				Mail mail = new Mail(from, model.Subject, to, content) { TemplateId = SendGridTemplateIds.PromoterReviewNotificationTemplateId };
				// add substitutions
				mail.Personalization[0].AddSubstitution("-promoterName-", model.PromoterName);
				mail.Personalization[0].AddSubstitution("-businessName-", model.BusinessName);
				mail.Personalization[0].AddSubstitution("-name-", model.Name);
				mail.Personalization[0].AddSubstitution("-email-", model.Email);
				mail.Personalization[0].AddSubstitution("-phone-", model.Phone);
				mail.Personalization[0].AddSubstitution("-partyCount-", model.PartyCount.ToString());
				mail.Personalization[0].AddSubstitution("-femaleCount-", model.FemaleCount.ToString());
				mail.Personalization[0].AddSubstitution("-maleCount-", model.MaleCount.ToString());
				mail.Personalization[0].AddSubstitution("-acceptLink-", model.AcceptLink);
				mail.Personalization[0].AddSubstitution("-declineLink-", model.DeclineLink);
				mail.Personalization[0].AddSubstitution("-date-", model.Date);

				// check if there is a Bcc defined
				if (model.Bccs != null) {
					foreach (var bccEmail in model.Bccs) {
						mail.Personalization[0].AddBcc(new Email(bccEmail));
					}
				}
				
				await SendEmail(mail);
			});
		}
	}
}
