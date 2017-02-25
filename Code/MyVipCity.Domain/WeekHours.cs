namespace MyVipCity.Domain {

	public class WeekHours {

		public int Id
		{
			get;
			set;
		}

		public virtual DayHours Monday
		{
			get;
			set;
		}

		public virtual DayHours Tuesday
		{
			get;
			set;
		}

		public virtual DayHours Wednesday
		{
			get;
			set;
		}

		public virtual DayHours Thursday
		{
			get;
			set;
		}

		public virtual DayHours Friday
		{
			get;
			set;
		}

		public virtual DayHours Saturday
		{
			get;
			set;
		}

		public virtual DayHours Sunday
		{
			get;
			set;
		}
	}
}
