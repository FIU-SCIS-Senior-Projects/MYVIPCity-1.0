namespace MyVipCity.DataTransferObjects.Search {

	public class BusinessSearchResultDto {

		public BusinessDto[] Businesses
		{
			get;
			set;
		}

		public double[] DistancesToReferenceCoordinate
		{
			get;
			set;
		}

		public BusinessSearchCriteriaDto SearchCriteria
		{
			get;
			set;
		}
	}
}
