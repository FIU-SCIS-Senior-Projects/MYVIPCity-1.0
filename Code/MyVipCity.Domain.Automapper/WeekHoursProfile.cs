using AutoMapper;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Domain.Automapper {

	public class WeekHoursProfile: Profile {

		public WeekHoursProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<WeekHours, WeekHoursDto>();
		}

		private void DtoToModel() {
			CreateMap<WeekHoursDto, WeekHours>();
		}
	}
}
