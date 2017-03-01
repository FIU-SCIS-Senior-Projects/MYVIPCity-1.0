using MyVipCity.Domain.Contracts;

namespace MyVipCity.Domain {

	public class File : IIdentifiable {

		// TODO Change Id to guid
		public int Id
		{
			get;
			set;
		}

		// TODO Change to GUID
		public int BinaryDataId
		{
			get;
			set;
		}

		public string FileName
		{
			get;
			set;
		}

		public string ContentType
		{
			get;
			set;
		}
	}
}
