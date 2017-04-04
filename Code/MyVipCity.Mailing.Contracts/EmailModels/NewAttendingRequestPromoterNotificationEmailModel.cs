namespace MyVipCity.Mailing.Contracts.EmailModels {

	public class NewAttendingRequestPromoterNotificationEmailModel: BasicEmailModel {

		public string PromoterName
		{
			get;
			set;
		}

		public string BusinessName
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string Phone
		{
			get;
			set;
		}

		public int PartyCount
		{
			get;
			set;
		}

		public int MaleCount
		{
			get;
			set;
		}

		public int FemaleCount
		{
			get;
			set;
		}

		public string Date
		{
			get;
			set;
		}

		public string Service
		{
			get;
			set;
		}

		public string Message
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
