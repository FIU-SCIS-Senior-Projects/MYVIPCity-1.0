namespace MyVipCity.Mailing.Contracts.EmailModels.AttendingRequest {

	public class DeclinedAttendingRequestAdminNotificationEmailModel: AttendingRequestAdminNotificationEmailModel {

		public string VipHostName
		{
			get;
			set;
		}
	}
}
