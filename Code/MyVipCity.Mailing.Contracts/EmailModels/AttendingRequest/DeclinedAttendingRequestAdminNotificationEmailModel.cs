namespace MyVipCity.Mailing.Contracts.EmailModels.AttendingRequest {

	public class DeclinedAttendingRequestAdminNotificationEmailModel: NewAttendingRequestAdminNotificationEmailModel {

		public string VipHostName
		{
			get;
			set;
		}
	}
}
