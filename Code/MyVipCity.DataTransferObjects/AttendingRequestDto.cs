using System;
using MyVipCity.DataTransferObjects.Contracts;

namespace MyVipCity.DataTransferObjects {

	public enum AttendingRequestServiceDto {
		VipTableService,
		PriorityGeneralEntry
	}

	public enum AttendingRequestStatusDto {
		Declined,
		Accepted,
		Pending
	}

	/// <summary>
	/// Used to store the information from a user who wants to attend a specific business.
	/// </summary>
	public class AttendingRequestDto: IIdentifiableDto {

		/// <summary>
		/// Id of the request.
		/// </summary>
		public int Id
		{
			get;
			set;
		}

		public AttendingRequestStatusDto Status
		{
			get; set;
		}

		/// <summary>
		/// Business.
		/// </summary>
		public virtual BusinessDto Business
		{
			get;
			set;
		}

		/// <summary>
		/// Name of the user making the request.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Email of the user making the request.
		/// </summary>
		public string Email
		{
			get;
			set;
		}

		/// <summary>
		/// Service desired.
		/// </summary>
		public AttendingRequestServiceDto DesiredService
		{
			get;
			set;
		}

		/// <summary>
		/// Phone number of the user making the request.
		/// </summary>
		public string ContactNumber
		{
			get;
			set;
		}

		/// <summary>
		/// Message.
		/// </summary>
		public string Message
		{
			get; set;
		}

		/// <summary>
		/// Attending date.
		/// </summary>
		public DateTime Date
		{
			get;
			set;
		}

		/// <summary>
		/// The number of members in the party.
		/// </summary>
		public int PartyCount
		{
			get;
			set;
		}

		/// <summary>
		/// Number of male members in the party.
		/// </summary>
		public int MaleCount
		{
			get;
			set;
		}

		/// <summary>
		/// Number of female members in the party.
		/// </summary>
		public int FemaleCount
		{
			get; set;
		}

		/// <summary>
		/// Promoter desired by the user making the request.
		/// </summary>
		public virtual PromoterProfileDto DesiredPromoter
		{
			get;
			set;
		}

		/// <summary>
		/// The promoter actually asigned for the request.
		/// </summary>
		public virtual PromoterProfileDto Promoter
		{
			get;
			set;
		}

		/////// <summary>
		/////// List of promoters who have declined the attending request.
		/////// </summary>
		////public virtual ICollection<AttendingRequestDeclineDto> Declines
		////{
		////	get; set;
		////}
	}
}
