using AutoMapper;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Domain.Automapper {

	public class ReviewProfile: Profile {

		public ReviewProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<Review, ReviewDto>();
		}

		private void DtoToModel() {
			CreateMap<ReviewDto, Review>();
		}
	}
}
