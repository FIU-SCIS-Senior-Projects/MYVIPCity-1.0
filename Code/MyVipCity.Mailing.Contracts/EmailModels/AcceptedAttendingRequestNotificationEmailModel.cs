namespace MyVipCity.Mailing.Contracts.EmailModels {

	public class AcceptedAttendingRequestNotificationEmailModel: BasicEmailModel {

		public string Name
		{
			get;
			set;
		}

		public string BusinessName
		{
			get;
			set;
		}

		public string Date
		{
			get;
			set;
		}

		public int PartyCount
		{
			get;
			set;
		}

		public string VipHostName
		{
			get;
			set;
		}

		public string VipHostPageLink
		{
			get;
			set;
		}
	}
}
