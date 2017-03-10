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
				.Include<Picture, PictureDto>()
				.Include<ProfilePicture, ProfilePictureDto>()
				.Include<IndexedPicture, IndexedPictureDto>();

			CreateMap<Picture, PictureDto>()
				.Include<ProfilePicture, ProfilePictureDto>()
				.Include<IndexedPicture, IndexedPictureDto>();

			CreateMap<IndexedPicture, IndexedPictureDto>();

			CreateMap<ProfilePicture, ProfilePictureDto>();
		}

		private void DtoToModel() {
			CreateMap<FileDto, File>()
				.Include<PictureDto, Picture>()
				.Include<ProfilePictureDto, ProfilePicture>()
				.Include<IndexedPictureDto, IndexedPicture>();

			// EquilityComparision is used to properly map ICollection<PictureDto> -> ICollection<Picture>, i.e in BusinessDto.Pictures
			CreateMap<PictureDto, Picture>()
				.Include<ProfilePictureDto, ProfilePicture>()
				.Include<IndexedPictureDto, IndexedPicture>()
				.EqualityComparision((odto, o) => odto.Id != 0 && odto.Id == o.Id);

			CreateMap<IndexedPictureDto, IndexedPicture>()
				.EqualityComparision((odto, o) => odto.Id != 0 && odto.Id == o.Id);

			CreateMap<ProfilePictureDto, ProfilePicture>()
				.EqualityComparision((odto, o) => odto.Id != 0 && odto.Id == o.Id);
		}
	}
}
