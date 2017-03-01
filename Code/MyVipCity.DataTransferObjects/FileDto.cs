using MyVipCity.DataTransferObjects.Contracts;

namespace MyVipCity.DataTransferObjects {

	public class FileDto : IIdentifiableDto {

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
