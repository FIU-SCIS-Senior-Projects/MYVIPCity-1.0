using System.Collections.Generic;
using MyVipCity.Domain.Social;

namespace MyVipCity.Domain.Contracts {

	public interface IPostsEntity {

		ICollection<Post> Posts
		{
			get;
		}
	}
}
