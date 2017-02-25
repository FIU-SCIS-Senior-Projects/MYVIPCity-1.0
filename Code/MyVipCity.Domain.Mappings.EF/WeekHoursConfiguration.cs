using System.Data.Entity.ModelConfiguration;

namespace MyVipCity.Domain.Mappings.EF {

	public class WeekHoursConfiguration: EntityTypeConfiguration<WeekHours> {

		public WeekHoursConfiguration() {
			// Id
			HasKey(w => w.Id);

			// properties

			// relationships
			HasRequired(w => w.Monday).WithMany().Map(m => m.MapKey("MondayId")).WillCascadeOnDelete(false);
			HasRequired(w => w.Tuesday).WithMany().Map(m => m.MapKey("TuesdayId")).WillCascadeOnDelete(false);
			HasRequired(w => w.Wednesday).WithMany().Map(m => m.MapKey("WednesdayId")).WillCascadeOnDelete(false);
			HasRequired(w => w.Thursday).WithMany().Map(m => m.MapKey("ThursdayId")).WillCascadeOnDelete(false);
			HasRequired(w => w.Friday).WithMany().Map(m => m.MapKey("FridayId")).WillCascadeOnDelete(false);
			HasRequired(w => w.Saturday).WithMany().Map(m => m.MapKey("SaturdayId")).WillCascadeOnDelete(false);
			HasRequired(w => w.Sunday).WithMany().Map(m => m.MapKey("SundayId")).WillCascadeOnDelete(false);
		}
	}
}
