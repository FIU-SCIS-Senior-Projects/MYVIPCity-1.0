namespace MyVipCity.Mailing.Contracts.EmailModels {

	public class ConfirmationEmailModel: BasicEmailModel {

		public string ConfirmationLink
		{
			get;
			set;
		}
	}
}
