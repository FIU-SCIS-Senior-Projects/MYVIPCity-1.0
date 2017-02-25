using System.Data.Entity.ModelConfiguration;

namespace MyVipCity.Domain.Mappings.EF {

	public class WeekHoursConfiguration: EntityTypeConfiguration<WeekHours> {

		public WeekHoursConfiguration() {
			// Id
			HasKey(w => w.Id);

			// properties

			// relationships
			HasRequired(w => w.Monday).WithRequiredDependent().Map(m => m.MapKey("MondayId"));
			HasRequired(w => w.Tuesday).WithRequiredDependent().Map(m => m.MapKey("TuesdayId"));
			HasRequired(w => w.Wednesday).WithRequiredDependent().Map(m => m.MapKey("WednesdayId"));
			HasRequired(w => w.Thursday).WithRequiredDependent().Map(m => m.MapKey("ThursdayId"));
			HasRequired(w => w.Friday).WithRequiredDependent().Map(m => m.MapKey("FridayId"));
			HasRequired(w => w.Saturday).WithRequiredDependent().Map(m => m.MapKey("SaturdayId"));
			HasRequired(w => w.Sunday).WithRequiredDependent().Map(m => m.MapKey("SundayId"));
		}
	}
}
