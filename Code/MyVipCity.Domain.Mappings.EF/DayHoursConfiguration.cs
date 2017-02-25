using System.Data.Entity.ModelConfiguration;

namespace MyVipCity.Domain.Mappings.EF {

	public class DayHoursConfiguration: EntityTypeConfiguration<DayHours> {

		public DayHoursConfiguration() {
			// Id
			HasKey(d => d.Id);

			// properties
			Property(d => d.Closed).IsRequired();
			Property(d => d.Open24).IsRequired();
			Property(d => d.Day).IsRequired();
		}
	}
}
