using System;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.Domain.Social {

	public abstract class Post : IIdentifiable {

		public int Id
		{
			get;
			set;
		}

		public DateTimeOffset PostedOn
		{
			get;
			set;
		}

		public string PostedBy
		{
			get;
			set;
		}
	}
}
