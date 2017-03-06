using AutoMapper;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain.Automapper.CustomResolvers;
using Ninject;

namespace MyVipCity.Domain.Automapper {

	public class PromoterProfileProfile: Profile {

		public PromoterProfileProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<PromoterProfile, PromoterProfileDto>();
		}

		private void DtoToModel() {
			CreateMap<PromoterProfileDto, PromoterProfile>()
				.ForMember(p => p.Business, opts => opts.ResolveUsing(new ReferenceValueResolver<PromoterProfileDto, PromoterProfile, BusinessDto, Business>(x => x.Business)))
				.ForMember(p => p.ProfilePicture, opts => opts.ResolveUsing(new ReferenceValueResolver<PromoterProfileDto, PromoterProfile, PictureDto, Picture>(x => x.ProfilePicture)));
		}
	}
}
