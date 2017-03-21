using System.Data.Entity.ModelConfiguration;
using MyVipCity.Domain.Social;

namespace MyVipCity.Domain.Mappings.EF.Social {

	public class VideoPostConfiguration: EntityTypeConfiguration<VideoPost> {

		public VideoPostConfiguration() {
			// Properties
			Property(c => c.Comment).HasMaxLength(1000).IsOptional();
		}
	}
}
