using System.Data.Entity.ModelConfiguration;

namespace MyVipCity.Domain.Mappings.EF {

	public class PromoterProfileConfiguration: EntityTypeConfiguration<PromoterProfile> {

		public PromoterProfileConfiguration() {
			// Id
			HasKey(p => p.Id);

			// properties
			Property(p => p.FirstName).HasMaxLength(100).IsRequired();
			Property(p => p.LastName).HasMaxLength(100);
			Property(p => p.NickName).HasMaxLength(100);
			Property(p => p.About).HasMaxLength(100000);
			Property(p => p.UserId).IsRequired();

			// relationships
			HasRequired(p => p.Business).WithMany();

			HasMany(p => p.Reviews).WithMany().Map(m => {
				m.ToTable("PromoterProfileReviews");
				m.MapLeftKey("PromoterProfileId");
				m.MapRightKey("ReviewId");
			});

			HasMany(b => b.Posts).WithMany().Map(m => {
				m.ToTable("PromoterProfilesPosts");
				m.MapLeftKey("PromoterProfileId");
				m.MapRightKey("PostId");
			});
		}
	}
}
