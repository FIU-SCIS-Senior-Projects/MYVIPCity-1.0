namespace MyVipCity.Mailing.Contracts.EmailModels {

	public class PromoterReviewNotificationEmailModel: BasicEmailModel {

		public string BusinessName
		{
			get;
			set;
		}

		public string Rating
		{
			get;
			set;
		}

		public string Comment
		{
			get;
			set;
		}
	}
}
