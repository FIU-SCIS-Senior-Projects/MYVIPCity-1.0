namespace MyVipCity.Mailing.Contracts.EmailModels {

	public class ForgotPasswordEmailModel: BasicEmailModel {

		public string ResetPasswordLink
		{
			get;
			set;
		}
	}
}
