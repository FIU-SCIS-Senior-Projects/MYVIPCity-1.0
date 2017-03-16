using System;
using MyVipCity.DataTransferObjects.Contracts;

namespace MyVipCity.DataTransferObjects {

	public class ReviewDto : IIdentifiableDto {

		public int Id
		{
			get;
			set;
		}

		public string Text
		{
			get;
			set;
		}

		public decimal Rating
		{
			get;
			set;
		}

		public DateTimeOffset CreatedOn
		{
			get;
			set;
		}

		public string ReviewerEmail
		{
			get;
			set;
		}
	}
}
