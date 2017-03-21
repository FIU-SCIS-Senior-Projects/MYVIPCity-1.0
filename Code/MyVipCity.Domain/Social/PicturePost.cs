using System.Collections.Generic;

namespace MyVipCity.Domain.Social {

	public class PicturePost: CommentPost {

		private ICollection<IndexedPicture> pictures;

		public PicturePost() {
			pictures = new List<IndexedPicture>();
		}

		public virtual ICollection<IndexedPicture> Pictures
		{
			get { return pictures; }
			protected set { pictures = value; }
		}
	}
}
