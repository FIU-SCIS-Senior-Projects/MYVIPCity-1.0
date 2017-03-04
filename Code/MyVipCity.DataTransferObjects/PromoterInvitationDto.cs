using System;
using MyVipCity.DataTransferObjects.Contracts;

namespace MyVipCity.DataTransferObjects {

	public enum PromoterInvitationStatusDto {
		New = 0,
		Sent = 1,
		Accepted = 2,
		Declined = 3
	}

	public class PromoterInvitationDto: IIdentifiableDto {

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

		public PromoterInvitationStatusDto Status
		{
			get;
			set;
		}
	}
}
