namespace MyVipCity.BusinessLogic.Contracts {

	public class ResultDto<T> {

		public T Result
		{
			get;
			set;
		}

		public string[] Messages
		{
			get;
			set;
		}

		public bool Error
		{
			get;
			set;
		}
	}
}
