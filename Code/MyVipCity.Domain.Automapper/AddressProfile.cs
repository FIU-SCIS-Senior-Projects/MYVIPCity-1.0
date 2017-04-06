using System.Data.Entity.Spatial;
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
			CreateMap<AddressDto, Address>()
				.ForMember(dest => dest.Location, opts => opts.MapFrom(src => DbGeography.FromText($"POINT({src.Longitude} {src.Latitude})")));
		}
	}
}
