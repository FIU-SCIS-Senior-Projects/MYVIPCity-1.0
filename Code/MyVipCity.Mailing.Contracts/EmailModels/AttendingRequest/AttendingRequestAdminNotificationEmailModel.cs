namespace MyVipCity.Mailing.Contracts.EmailModels.AttendingRequest {

	public class AttendingRequestAdminNotificationEmailModel : AttendingRequestNotificationEmailModel {

		public string AdminName
		{
			get;
			set;
		}

		public string AssignVipHostUrl
		{
			get;
			set;
		}
	}
}
