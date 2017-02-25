using System.Collections;
using System.Collections.Generic;

namespace MyVipCity.Domain {

	public class Business {

		public int Id
		{
			get;
			set;
		}

		public string FriendlyIdBase
		{
			get;
			set;
		}

		public string FriendlyId
		{
			get;
			set;
		}

		/// <summary>
		/// Name of the business.
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Phrase of the business. This is a motto.
		/// </summary>
		public string Phrase
		{
			get;
			set;
		}

		/// <summary>
		/// Brief description of the Ambiance of the business.
		/// </summary>
		public string Ambiance
		{
			get;
			set;
		}

		/// <summary>
		/// Parking information.
		/// </summary>
		public string Parking
		{
			get;
			set;
		}

		/// <summary>
		/// Alcohol information.
		/// </summary>
		public string Alcohol
		{
			get;
			set;
		}

		/// <summary>
		/// Phone information.
		/// </summary>
		public string Phone
		{
			get;
			set;
		}

		/// <summary>
		/// Website URL.
		/// </summary>
		public string WebsiteUrl
		{
			get;
			set;
		}

		/// <summary>
		/// Indicates whether or not the business is good for dancing.
		/// </summary>
		public bool GoodForDancing
		{
			get;
			set;
		}

		/// <summary>
		/// Details of the business.
		/// </summary>
		public string Details
		{
			get;
			set;
		}

		/// <summary>
		/// List of amenities separated by comma(,).
		/// </summary>
		public string Amenities
		{
			get;
			set;
		}

		/// <summary>
		/// A small phrase to introduce the <see cref="Amenities"/>.
		/// </summary>
		public string AmenitiesPhrase
		{
			get;
			set;
		}

		/// <summary>
		/// Week hours for the business.
		/// </summary>
		public virtual WeekHours WeekHours
		{
			get;
			set;
		}

		/// <summary>
		/// Address information of the business.
		/// </summary>
		public Address Address
		{
			get;
			set;
		}

		/// <summary>
		/// List of pictures for the business.
		/// </summary>
		public virtual ICollection<Picture> Pictures
		{
			get;
			set;
		}
	}
}
