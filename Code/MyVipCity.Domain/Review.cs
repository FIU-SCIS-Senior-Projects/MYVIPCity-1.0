using System;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.Domain {

	public class Review : IIdentifiable {

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
