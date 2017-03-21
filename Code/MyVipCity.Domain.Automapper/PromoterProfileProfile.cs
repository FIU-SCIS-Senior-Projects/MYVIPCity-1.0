using System.Linq;
using AutoMapper;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain.Automapper.CustomResolvers;

namespace MyVipCity.Domain.Automapper {

	public class PromoterProfileProfile: Profile {

		public PromoterProfileProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<PromoterProfile, PromoterProfileDto>()
				.ForMember(p => p.ReviewsCount,
					opts => {
						// guarantee that this property is mapped before mapping Average rating
						opts.SetMappingOrder(0);
						opts.MapFrom(src => src.Reviews == null ? 0 : src.Reviews.Count());
					})
				.ForMember(p => p.AverageRating,
					opts => {
						// guarantee that this property is mapped after mapping ReviewsCount
						opts.SetMappingOrder(1);
						// if there are no reviews give a courtesy 5 star reviews to the profile, otherwise just average the reviews rating
						opts.ResolveUsing((profile, dto) => dto.ReviewsCount == 0 ? 5 : profile.Reviews.Average(r => r.Rating));
					});
		}

		private void DtoToModel() {
			CreateMap<PromoterProfileDto, PromoterProfile>()
				.ForMember(p => p.CreatedOn, opts => opts.Ignore())
				.ForMember(p => p.Reviews, opts => opts.Ignore())
				.ForMember(p => p.Business, opts => opts.ResolveUsing(new ReferenceValueResolver<PromoterProfileDto, PromoterProfile, BusinessDto, Business>(x => x.Business)))
				.ForMember(p => p.ProfilePicture, opts => opts.ResolveUsing(new ReferenceValueResolver<PromoterProfileDto, PromoterProfile, ProfilePictureDto, ProfilePicture>(x => x.ProfilePicture)));
		}
	}
}
