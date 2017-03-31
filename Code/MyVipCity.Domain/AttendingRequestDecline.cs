using System;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.Domain {

	public class AttendingRequestDecline: IIdentifiable {

		/// <summary>
		/// Id.
		/// </summary>
		public int Id
		{
			get;
			set;
		}

		/// <summary>
		/// Attending request that was declined.
		/// </summary>
		public virtual AttendingRequest AttendingRequest
		{
			get;
			set;
		}

		/// <summary>
		/// Promoter who declined the attending request.
		/// </summary>
		public virtual PromoterProfile Promoter
		{
			get;
			set;
		}

		/// <summary>
		/// Date when the decline occurred.
		/// </summary>
		public DateTimeOffset DeclinedOn
		{
			get;
			set;
		}

		/// <summary>
		/// Decline reason.
		/// </summary>
		public string DeclineReason
		{
			get;
			set;
		}
	}
}
