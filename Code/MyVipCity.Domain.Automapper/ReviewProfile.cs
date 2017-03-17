using System;
using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Domain.Automapper {

	public class ReviewProfile: Profile {

		public ReviewProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<Review, ReviewDto>()
				.ForMember(dest => dest.ReviewerEmail, opts => opts.MapFrom(src => src.ReviewerEmail.Substring(0, src.ReviewerEmail.IndexOf("@", StringComparison.Ordinal))));
		}

		private void DtoToModel() {
			CreateMap<ReviewDto, Review>();
		}
	}
}
