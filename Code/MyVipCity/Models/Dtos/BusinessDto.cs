namespace MyVipCity.Models.Dtos {

	public class BusinessDto {

		/// <summary>
		/// Name of the business.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Phrase of the business. This is a motto.
		/// </summary>
		public string Phrase
		{
			get;
			set;
		}

		/// <summary>
		/// Brief description of the Ambiance of the business.
		/// </summary>
		public string Ambiance
		{
			get;
			set;
		}

		/// <summary>
		/// Parking information.
		/// </summary>
		public string Parking
		{
			get;
			set;
		}

		/// <summary>
		/// Alcohol information.
		/// </summary>
		public string Alcohol
		{
			get;
			set;
		}

		/// <summary>
		/// Website URL.
		/// </summary>
		public string WebsiteUrl
		{
			get;
			set;
		}
	}
}