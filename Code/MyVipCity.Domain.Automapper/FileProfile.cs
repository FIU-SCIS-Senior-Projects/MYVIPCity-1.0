using AutoMapper;
using AutoMapper.EquivilencyExpression;
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

			// EquilityComparision is used to properly map ICollection<PictureDto> -> ICollection<Picture>, i.e in BusinessDto.Pictures
			CreateMap<PictureDto, Picture>()
				.EqualityComparision((odto, o) => odto.Id != 0 && odto.Id == o.Id);
		}
	}
}
