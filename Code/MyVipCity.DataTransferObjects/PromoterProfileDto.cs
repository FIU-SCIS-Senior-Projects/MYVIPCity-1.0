using System;
using MyVipCity.DataTransferObjects.Contracts;

namespace MyVipCity.DataTransferObjects {

	public class PromoterProfileDto: IIdentifiableDto {

		public int Id
		{
			get;
			set;
		}

		public string UserId
		{
			get;
			set;
		}

		public BusinessDto Business
		{
			get;
			set;
		}

		public string FirstName
		{
			get;
			set;
		}

		public string LastName
		{
			get;
			set;
		}

		public string NickName
		{
			get;
			set;
		}

		public string About
		{
			get;
			set;
		}

		public ProfilePictureDto ProfilePicture
		{
			get;
			set;
		}

		public DateTimeOffset CreatedOn
		{
			get;
			set;
		}
	}
}
