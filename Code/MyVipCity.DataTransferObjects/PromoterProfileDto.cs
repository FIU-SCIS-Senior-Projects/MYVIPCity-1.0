using System;
using MyVipCity.DataTransferObjects.Contracts;

namespace MyVipCity.DataTransferObjects {

	public class PromoterProfileDto: IIdentifiableDto {

		/// <summary>
		/// Unique identifier for the profile.
		/// </summary>
		public int Id
		{
			get;
			set;
		}

		/// <summary>
		/// Business associated to the profile.
		/// </summary>
		public BusinessDto Business
		{
			get;
			set;
		}


		/// <summary>
		/// First name of the promoter.
		/// </summary>
		public string FirstName
		{
			get;
			set;
		}

		/// <summary>
		/// Last name of the promoter.
		/// </summary>
		public string LastName
		{
			get;
			set;
		}

		/// <summary>
		/// Nickname of the promoter.
		/// </summary>
		public string NickName
		{
			get;
			set;
		}

		/// <summary>
		/// Information about the promoter.
		/// </summary>
		public string About
		{
			get;
			set;
		}

		/// <summary>
		/// Profile picture of the promoter.
		/// </summary>
		public ProfilePictureDto ProfilePicture
		{
			get;
			set;
		}

		/// <summary>
		/// Date and time in UTC where the profile was created.
		/// </summary>
		public DateTimeOffset CreatedOn
		{
			get;
			set;
		}

		/// <summary>
		/// Number of reviews for this promoter profile.
		/// </summary>
		public int ReviewsCount
		{
			get;
			set;
		}

		/// <summary>
		/// Average reviews rating for this promoter profile.
		/// </summary>
		public decimal AverageRating
		{
			get;
			set;
		}
	}
}
