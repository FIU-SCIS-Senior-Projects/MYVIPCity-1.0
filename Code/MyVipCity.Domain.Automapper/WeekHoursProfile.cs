using AutoMapper;
using MyVipCity.DataTransferObjects;
using MyVipCity.Domain.Automapper.CustomResolvers;

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
			CreateMap<WeekHoursDto, WeekHours>()
				.ForMember(w => w.Monday, opts => opts.ResolveUsing(new ReferenceValueResolver<WeekHoursDto, WeekHours, DayHoursDto, DayHours>(we => we.Monday)))
				.ForMember(w => w.Tuesday, opts => opts.ResolveUsing(new ReferenceValueResolver<WeekHoursDto, WeekHours, DayHoursDto, DayHours>(we => we.Tuesday)))
				.ForMember(w => w.Wednesday, opts => opts.ResolveUsing(new ReferenceValueResolver<WeekHoursDto, WeekHours, DayHoursDto, DayHours>(we => we.Wednesday)))
				.ForMember(w => w.Thursday, opts => opts.ResolveUsing(new ReferenceValueResolver<WeekHoursDto, WeekHours, DayHoursDto, DayHours>(we => we.Thursday)))
				.ForMember(w => w.Friday, opts => opts.ResolveUsing(new ReferenceValueResolver<WeekHoursDto, WeekHours, DayHoursDto, DayHours>(we => we.Friday)))
				.ForMember(w => w.Saturday, opts => opts.ResolveUsing(new ReferenceValueResolver<WeekHoursDto, WeekHours, DayHoursDto, DayHours>(we => we.Saturday)))
				.ForMember(w => w.Sunday, opts => opts.ResolveUsing(new ReferenceValueResolver<WeekHoursDto, WeekHours, DayHoursDto, DayHours>(we => we.Sunday)));
		}
	}
}

