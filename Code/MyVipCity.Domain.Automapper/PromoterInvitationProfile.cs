using AutoMapper;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Domain.Automapper {

	public class PromoterInvitationProfile: Profile {

		public PromoterInvitationProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<PromoterInvitation, PromoterInvitationDto>();
		}

		private void DtoToModel() {
			CreateMap<PromoterInvitationDto, PromoterInvitation>();
		}
	}
}
