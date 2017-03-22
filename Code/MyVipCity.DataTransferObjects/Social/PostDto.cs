using System;
using MyVipCity.DataTransferObjects.Contracts;

namespace MyVipCity.DataTransferObjects.Social {

	public abstract class PostDto: IIdentifiableDto {

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

		public string PostType
		{
			get;
			set;
		}
	}
}
