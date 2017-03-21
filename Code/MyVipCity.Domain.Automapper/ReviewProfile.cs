using System;
using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using AutoMapper.EquivilencyExpression;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Domain.Automapper {

	public class ReviewProfile: Profile {

		public ReviewProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<Review, ReviewDto>()
				.ForMember(dest => dest.ReviewerEmail, opts => opts.MapFrom(src => src.ReviewerEmail.Substring(0, src.ReviewerEmail.IndexOf("@", StringComparison.Ordinal))))
				.EqualityComparision((odto, o) => odto.Id != 0 && odto.Id == o.Id);
		}

		private void DtoToModel() {
			CreateMap<ReviewDto, Review>();
		}
	}
}
