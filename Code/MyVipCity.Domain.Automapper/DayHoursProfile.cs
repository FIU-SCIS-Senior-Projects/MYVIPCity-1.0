using AutoMapper;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Domain.Automapper {

	public class DayHoursProfile: Profile {

		public DayHoursProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<DayHours, DayHoursDto>();
		}

		private void DtoToModel() {
			CreateMap<DayHoursDto, DayHours>();
		}
	}
}
