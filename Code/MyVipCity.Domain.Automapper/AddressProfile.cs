using AutoMapper;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Domain.Automapper {

	public class AddressProfile: Profile {

		public AddressProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<Address, AddressDto>();
		}

		private void DtoToModel() {
			CreateMap<AddressDto, Address>();
		}
	}
}
