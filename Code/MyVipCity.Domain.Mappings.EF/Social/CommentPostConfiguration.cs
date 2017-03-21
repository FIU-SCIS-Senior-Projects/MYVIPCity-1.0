using System.Data.Entity.ModelConfiguration;
using MyVipCity.Domain.Social;

namespace MyVipCity.Domain.Mappings.EF.Social {

	public class CommentPostConfiguration: EntityTypeConfiguration<CommentPost> {

		public CommentPostConfiguration() {
			// Properties
			Property(c => c.Comment).HasMaxLength(1000);
		}
	}
}
