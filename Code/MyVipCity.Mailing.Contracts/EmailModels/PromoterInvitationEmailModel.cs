namespace MyVipCity.Mailing.Contracts.EmailModels {

	public class PromoterInvitationEmailModel: BasicEmailModel {

		public string Name
		{
			get;
			set;
		}

		public string ClubName
		{
			get;
			set;
		}

		public string AcceptInvitationUrl
		{
			get;
			set;
		}
	}
}
