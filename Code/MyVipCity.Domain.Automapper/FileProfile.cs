using AutoMapper;
using MyVipCity.DataTransferObjects;

namespace MyVipCity.Domain.Automapper {

	public class FileProfile: Profile {

		public FileProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void ModelToDto() {
			CreateMap<File, FileDto>()
				.Include<Picture, PictureDto>();

			CreateMap<Picture, PictureDto>();
		}

		private void DtoToModel() {
			CreateMap<FileDto, File>()
				.Include<PictureDto, Picture>();

			CreateMap<PictureDto, Picture>();
		}
	}
}
