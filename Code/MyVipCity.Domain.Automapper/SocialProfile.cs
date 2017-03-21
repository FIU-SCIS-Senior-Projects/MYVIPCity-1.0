﻿using AutoMapper;
using AutoMapper.EquivilencyExpression;
using MyVipCity.DataTransferObjects.Social;
using MyVipCity.Domain.Social;

namespace MyVipCity.Domain.Automapper {

	public class SocialProfile: Profile {

		public SocialProfile() {
			DtoToModel();
			ModelToDto();
		}

		private void DtoToModel() {
			CreateMap<PostDto, Post>()
				.Include<CommentPostDto, CommentPost>()
				.Include<VideoPostDto, VideoPost>()
				.Include<PicturePostDto, PicturePost>()
				.EqualityComparision((odto, o) => odto.Id != 0 && odto.Id == o.Id);

			CreateMap<CommentPostDto, CommentPost>()
				.Include<VideoPostDto, VideoPost>()
				.Include<PicturePostDto, PicturePost>()
				.EqualityComparision((odto, o) => odto.Id != 0 && odto.Id == o.Id);

			CreateMap<VideoPostDto, VideoPost>()
				.EqualityComparision((odto, o) => odto.Id != 0 && odto.Id == o.Id);

			CreateMap<PicturePostDto, PicturePost>()
				.EqualityComparision((odto, o) => odto.Id != 0 && odto.Id == o.Id);
		}

		private void ModelToDto() {
			CreateMap<Post, PostDto>()
				.Include<CommentPost, CommentPostDto>()
				.Include<VideoPost, VideoPostDto>()
				.Include<PicturePost, PicturePostDto>();

			CreateMap<CommentPost, CommentPostDto>()
				.Include<VideoPost, VideoPostDto>()
				.Include<PicturePost, PicturePostDto>();

			CreateMap<VideoPost, VideoPostDto>();

			CreateMap<PicturePost, PicturePostDto>();
		}
	}
}
