using System.Data.Entity.ModelConfiguration;

namespace MyVipCity.Domain.Mappings.EF {

	public class WeekHoursConfiguration: EntityTypeConfiguration<WeekHours> {

		public WeekHoursConfiguration() {
			// Id
			HasKey(w => w.Id);

			// properties

			// relationships
			HasRequired(w => w.Monday).WithOptional().Map(m => m.MapKey("MondayId"));
			HasRequired(w => w.Tuesday).WithOptional().Map(m => m.MapKey("TuesdayId"));
			HasRequired(w => w.Wednesday).WithOptional().Map(m => m.MapKey("WednesdayId"));
			HasRequired(w => w.Thursday).WithOptional().Map(m => m.MapKey("ThursdayId"));
			HasRequired(w => w.Friday).WithOptional().Map(m => m.MapKey("FridayId"));
			HasRequired(w => w.Saturday).WithOptional().Map(m => m.MapKey("SaturdayId"));
			HasRequired(w => w.Sunday).WithOptional().Map(m => m.MapKey("SundayId"));
		}
	}
}
