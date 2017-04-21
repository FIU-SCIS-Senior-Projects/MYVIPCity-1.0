using MyVipCity.DataTransferObjects.Social;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IPostsEntityManager {

		PostDto AddPost<T>(int entityId, PostDto postDto) where T : class, IPostsEntity;

		PostDto UpdatePost<T>(int entityId, PostDto postDto) where T : class, IPostsEntity;

		PostDto AddOrUpdatePost<T>(int entityId, PostDto postDto) where T : class, IPostsEntity;

		ResultDto<bool> DeletePost<T>(int entityId, int postId) where T : class, IPostsEntity;

		PostDto[] GetPosts<T>(int entityId, int top) where T : class, IPostsEntity;

		PostDto[] GetPosts<T>(int entityId, int top, int afterPostentityId) where T : class, IPostsEntity;
	}
}