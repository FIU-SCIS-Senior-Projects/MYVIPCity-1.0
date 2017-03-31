using System.Data.Entity.ModelConfiguration;

namespace MyVipCity.Domain.Mappings.EF {

	public class AttendingRequestConfiguration: EntityTypeConfiguration<AttendingRequest> {

		public AttendingRequestConfiguration() {
			// Id
			HasKey(r => r.Id);

			// properties
			Property(r => r.Message).HasMaxLength(1000);
			Property(r => r.Email).HasMaxLength(256);
			Property(r => r.UserId).HasMaxLength(128);

			// relationships
			HasRequired(r => r.Business).WithMany();
			HasOptional(r => r.DesiredPromoter).WithMany();
			HasOptional(r => r.Promoter).WithMany();
		}
	}
}
