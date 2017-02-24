using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace MyVipCity.Domain.Mappings.EF {
	public class FileConfiguration: EntityTypeConfiguration<File> {

		public FileConfiguration() {
			// Id
			HasKey(f => f.Id);

			// properties
			Property(b => b.FileName).IsRequired();
			Property(b => b.BinaryDataId)
				.IsRequired();
			Property(b => b.ContentType).IsRequired();

			Map(m => {
				m.ToTable("Files");
			});
		}
	}
}
