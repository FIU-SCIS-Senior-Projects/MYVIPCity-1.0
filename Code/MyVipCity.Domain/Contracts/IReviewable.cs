using System.Collections.Generic;

namespace MyVipCity.Domain.Contracts {

	public interface IReviewable {

		ICollection<Review> Reviews
		{
			get;
			set;
		}
	}
}
