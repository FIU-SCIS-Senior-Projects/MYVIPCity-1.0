using AutoMapper;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain.Automapper.CustomResolvers;
using Ninject;

namespace MyVipCity.Domain.Automapper {

	public class BusinessProfile: Profile {

		public BusinessProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<Business, BusinessDto>();
		}

		private void DtoToModel() {
			CreateMap<BusinessDto, Business>()
				.ForMember(b => b.WeekHours, opts => opts.ResolveUsing(new ReferenceValueResolver<BusinessDto, Business, WeekHoursDto, WeekHours>(bu => bu.WeekHours)));
		}
	}
}
