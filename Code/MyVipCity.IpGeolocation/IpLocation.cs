namespace MyVipCity.IpGeolocation {

	public class IpLocation {

		public string IpAddress
		{
			get;
			set;
		}

		public string CountryCode
		{
			get;
			set;
		}

		public string Country
		{
			get;
			set;
		}

		public string Region
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string ZipCode
		{
			get;
			set;
		}

		public decimal Latitude
		{
			get;
			set;
		}

		public decimal Longitude
		{
			get;
			set;
		}

		public string TimeZone
		{
			get;
			set;
		}

		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		public override string ToString() {
			return $"IpAddress: {IpAddress}\n" +
				   $"Country Code: {CountryCode}\n" +
				   $"Country: {Country}\n" +
				   $"Region: {Region}\n" +
				   $"City: {City}\n" +
				   $"Zip Code: {ZipCode}\n" +
				   $"Latitude: {Latitude}\n" +
				   $"Longitude: {Longitude}\n" +
				   $"TimeZone: {TimeZone}";
		}
	}
}
