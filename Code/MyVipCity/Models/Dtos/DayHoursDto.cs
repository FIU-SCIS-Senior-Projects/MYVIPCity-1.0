using System;

namespace MyVipCity.Models.Dtos {

	public enum WeekDayDto {
		Sunday = 0,
		Monday = 1,
		Tuesday = 2,
		Wednesday = 3,
		Thursday = 4,
		Friday = 5,
		Saturday = 6
	};

	public class DayHoursDto {

		public int Id
		{
			get;
			set;
		}

		public WeekDayDto Day
		{
			get;
			set;
		}

		public bool Open24
		{
			get;
			set;
		}

		public bool Closed
		{
			get;
			set;
		}

		public DateTime? OpenTime
		{
			get;
			set;
		}

		public DateTime? CloseTime
		{
			get;
			set;
		}
	}
}
