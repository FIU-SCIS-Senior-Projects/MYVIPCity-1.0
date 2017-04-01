using AutoMapper;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain.Automapper.CustomResolvers;
using MyVipCity.Domain.Automapper.Extensions;

namespace MyVipCity.Domain.Automapper {

	public class AttendingRequestProfile: Profile {

		public AttendingRequestProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<AttendingRequest, AttendingRequestDto>();
		}

		private void DtoToModel() {
			CreateMap<AttendingRequestDto, AttendingRequest>()
				.CheckNonZeroIdForMember(dest => dest.Business, src => src.Business)
				.ForMember(r => r.Business, opts => opts.ResolveUsing(new ReferenceValueResolver<AttendingRequestDto, AttendingRequest, BusinessDto, Business>(rDto => rDto.Business, loadOnly: true)))
				.CheckNonZeroIdForMember(dest => dest.Business, src => src.Promoter)
				.ForMember(r => r.Promoter, opts => opts.ResolveUsing(new ReferenceValueResolver<AttendingRequestDto, AttendingRequest, PromoterProfileDto, PromoterProfile>(rDto => rDto.Promoter, loadOnly: true)))
				.CheckNonZeroIdForMember(dest => dest.Business, src => src.DesiredPromoter)
				.ForMember(r => r.DesiredPromoter, opts => opts.ResolveUsing(new ReferenceValueResolver<AttendingRequestDto, AttendingRequest, PromoterProfileDto, PromoterProfile>(rDto => rDto.DesiredPromoter, loadOnly: true)));
		}
	}
}
