using System.Threading.Tasks;
using MyVipCity.Mailing.Contracts.EmailModels;
using MyVipCity.Mailing.Contracts.EmailModels.AttendingRequest;

namespace MyVipCity.Mailing.Contracts {

	public interface IEmailService {

		// Sends a regular email providing from, to, subject and body of the message
		Task SendBasicEmailAsync(BasicEmailModel model);

		// Sends a confirmation email to a just registered user
		Task SendConfirmationEmailAsync(ConfirmationEmailModel model);

		// Sends an email to reset a forgotten password.
		Task SendForgotPasswordEmailAsync(ForgotPasswordEmailModel model);

		// Sends an email to invite a promoter to join a club.
		Task SendPromoterInvitationEmailAsync(PromoterInvitationEmailModel model);

		// Sends an email to notify a new review has been submitted.
		Task SendPromoterReviewNotificationEmailAsync(PromoterReviewNotificationEmailModel model);

		// Sends an email to a promoter notifying he/she has been selected as the VIP host for an attending request.
		Task SendAttendigRequestNotificationToPromoterAsync(NewAttendingRequestPromoterNotificationEmailModel model);

		// Sends an email to an admin notifying he/she a new attending request without a selected promoter has been submitted.
		Task SendAttendigRequestNotificationToAdminAsync(NewAttendingRequestAdminNotificationEmailModel model);

		// Sends an email to the user who made an attending request notifying the request has been approved by the promoter.
		Task SendAcceptedAttendingRequestNotificationToUserAsync(AcceptedAttendingRequestNotificationEmailModel model);

		// Sends an email to an admin notifying about a declined attending request by a promoter. 
		Task SendDeclinedAttendingRequestNotificationToAdminAsync(DeclinedAttendingRequestAdminNotificationEmailModel model);
	}
}