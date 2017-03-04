using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.Domain {

	public enum PromoterInvitationStatus {
		New = 0,
		Sent = 1,
		Accepted = 2,
		Declined = 3
	}

	public class PromoterInvitation: IIdentifiable {

		public int Id
		{
			get;
			set;
		}

		public string Name
		{
			get;
			set;
		}

		public string Email
		{
			get;
			set;
		}

		public string ClubFriendlyId
		{
			get;
			set;
		}

		public DateTimeOffset SentOn
		{
			get;
			set;
		}

		public PromoterInvitationStatus Status
		{
			get;
			set;
		}
	}
}
