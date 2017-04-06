namespace MyVipCity.DataTransferObjects.Search {

	public class BusinessSearchCriteriaDto {

		public string Criteria
		{
			get;
			set;
		}

		public int Top
		{
			get; set;
		}

		public int Skip
		{
			get; set;
		}

		public decimal ReferenceLongitude
		{
			get;
			set;
		}

		public decimal ReferenceLatitude
		{
			get;
			set;
		}
	}
}
