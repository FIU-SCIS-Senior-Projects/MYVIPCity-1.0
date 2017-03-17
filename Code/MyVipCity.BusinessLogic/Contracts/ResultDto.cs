namespace MyVipCity.BusinessLogic.Contracts {

	public class ResultDto<T> {

		public ResultDto() {
			
		}

		public ResultDto(T result) {
			Result = result;
		}

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
