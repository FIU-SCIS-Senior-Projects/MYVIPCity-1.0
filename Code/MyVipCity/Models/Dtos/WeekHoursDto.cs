﻿namespace MyVipCity.Models.Dtos {

	public class WeekHoursDto {

		public int Id
		{
			get;
			set;
		}

		public DayHoursDto Monday
		{
			get;
			set;
		}

		public DayHoursDto Tuesday
		{
			get;
			set;
		}

		public DayHoursDto Wednesday
		{
			get;
			set;
		}

		public DayHoursDto Thursday
		{
			get;
			set;
		}
		public DayHoursDto Friday
		{
			get;
			set;
		}

		public DayHoursDto Saturday
		{
			get;
			set;
		}

		public DayHoursDto Sunday
		{
			get;
			set;
		}
	}
}
