using System.Data.Entity.ModelConfiguration;

namespace MyVipCity.Domain.Mappings.EF {

	public class PromoterInvitationConfiguration: EntityTypeConfiguration<PromoterInvitation> {

		public PromoterInvitationConfiguration() {
			// Id
			HasKey(i => i.Id);

			// properties
			Property(i => i.ClubFriendlyId).IsRequired();
			Property(i => i.Email).IsRequired();
			Property(i => i.Name).IsRequired();
			Property(i => i.Status).IsRequired();
		}
	}
}

