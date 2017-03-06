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
			Property(p => p.About).HasMaxLength(10000);
			Property(p => p.UserId).IsRequired();

			// relationships
			HasRequired(p => p.Business).WithMany();
		}
	}
}
