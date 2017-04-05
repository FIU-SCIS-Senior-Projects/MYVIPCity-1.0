namespace MyVipCity.Mailing.Contracts.EmailModels {

	public class NewAttendingRequestAdminNotificationEmailModel : NewAttendingRequestNotificationEmailModel {

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
