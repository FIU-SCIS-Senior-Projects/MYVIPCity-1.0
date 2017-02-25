using System.Data.Entity.ModelConfiguration;

namespace MyVipCity.Domain.Mappings.EF {

	public class BusinessConfiguration: EntityTypeConfiguration<Business> {

		public BusinessConfiguration() {
			// Id
			HasKey(b => b.Id);

			// properties
			Property(b => b.FriendlyId).IsRequired();
			Property(b => b.Name).IsRequired().HasMaxLength(150);
			Property(b => b.Phrase).HasMaxLength(500);
			Property(b => b.Ambiance).HasMaxLength(150);
			Property(b => b.Parking).HasMaxLength(150);
			Property(b => b.Alcohol).HasMaxLength(150);
			Property(b => b.WebsiteUrl).HasMaxLength(150);
			Property(b => b.GoodForDancing).IsRequired();
			Property(b => b.Amenities).HasMaxLength(2000);
			Property(b => b.AmenitiesPhrase).HasMaxLength(500);

			// relationships
			HasMany(b => b.Pictures).WithMany().Map(m => {
				m.ToTable("BusinessesFiles");
				m.MapLeftKey("BusinessId");
				m.MapRightKey("FileId");
			});
			HasRequired(b => b.WeekHours).WithRequiredDependent().Map(m => m.MapKey("WeekHoursId"));
		}
	}
}
