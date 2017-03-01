using System;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.Domain {

	public enum WeekDay {
		Sunday = 0,
		Monday = 1,
		Tuesday = 2,
		Wednesday = 3,
		Thursday = 4,
		Friday = 5,
		Saturday = 6
	};

	public class DayHours : IIdentifiable {

		public int Id
		{
			get;
			set;
		}

		public WeekDay Day
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
