﻿namespace MyVipCity.Models.Dtos {

	public class AddressDto {

		public string Street
		{
			get;
			set;
		}

		public string City
		{
			get;
			set;
		}

		public string State
		{
			get;
			set;
		}

		public string Country
		{
			get;
			set;
		}

		public string ZipCode
		{
			get;
			set;
		}

		public string FormattedAddress
		{
			get;
			set;
		}

		public decimal Longitude
		{
			get;
			set;
		}

		public decimal Latitude
		{
			get;
			set;
		}
	}
}
