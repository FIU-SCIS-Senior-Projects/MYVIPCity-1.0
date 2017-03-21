using System.Data.Entity.ModelConfiguration;
using MyVipCity.Domain.Social;

namespace MyVipCity.Domain.Mappings.EF.Social {

	public class PostConfiguration: EntityTypeConfiguration<Post> {

		public PostConfiguration() {
			// Id
			HasKey(p => p.Id);

			// Properties
			Property(p => p.PostedBy).HasMaxLength(256).IsRequired();
			Property(p => p.PostedOn).IsRequired();
		}
	}
}
