using System.Collections.Generic;

namespace MyVipCity.DataTransferObjects.Social {

	public class PicturePostDto: CommentPostDto {

		public PicturePostDto() {
			Pictures = new List<IndexedPictureDto>();
		}

		public ICollection<IndexedPictureDto> Pictures
		{
			get;
			protected set;
		}
	}
}
