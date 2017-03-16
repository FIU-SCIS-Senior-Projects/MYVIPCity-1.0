using System.Data.Entity.ModelConfiguration;

namespace MyVipCity.Domain.Mappings.EF {

	public class ReviewConfiguration: EntityTypeConfiguration<Review> {

		public ReviewConfiguration() {
			// Id
			HasKey(d => d.Id);

			// properties
			Property(d => d.Rating).IsRequired();
			Property(d => d.CreatedOn).IsRequired();
			Property(d => d.ReviewerEmail).IsRequired().HasMaxLength(256);
			Property(d => d.Text).HasMaxLength(3000);
		}
	}
}
