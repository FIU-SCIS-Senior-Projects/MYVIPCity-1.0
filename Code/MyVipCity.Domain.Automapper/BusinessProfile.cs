using AutoMapper;
using MyVipCity.DataTransferObjects;

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
			CreateMap<BusinessDto, Business>();
		}
	}
}
