using System.Data.Entity.ModelConfiguration;
using MyVipCity.Domain.Social;

namespace MyVipCity.Domain.Mappings.EF.Social {

	public class PicturePostConfiguration: EntityTypeConfiguration<PicturePost> {

		public PicturePostConfiguration() {
			// relationships
			HasMany(b => b.Pictures).WithMany().Map(m => {
				m.ToTable("PicturePostPictures");
				m.MapLeftKey("PicturePostId");
				m.MapRightKey("PictureId");
			});
		}
	}
}
