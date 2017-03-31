using System;
using MyVipCity.DataTransferObjects.Contracts;

namespace MyVipCity.DataTransferObjects {

	public class AttendingRequestDeclineDto: IIdentifiableDto {

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
		public virtual AttendingRequestDto AttendingRequest
		{
			get;
			set;
		}

		/// <summary>
		/// Promoter who declined the attending request.
		/// </summary>
		public virtual PromoterProfileDto Promoter
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
