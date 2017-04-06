namespace MyVipCity.Mailing.Contracts.EmailModels.AttendingRequest {

	public class AttendingRequestPromoterNotificationEmailModel: AttendingRequestNotificationEmailModel {

		public string PromoterName
		{
			get;
			set;
		}
		
		public string AcceptLink
		{
			get;
			set;
		}

		public string DeclineLink
		{
			get;
			set;
		}
	}
}
