namespace MyVipCity.Models.Dtos {

	public class UploadedFileDto {

		public int Id
		{
			get;
			set;
		}

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