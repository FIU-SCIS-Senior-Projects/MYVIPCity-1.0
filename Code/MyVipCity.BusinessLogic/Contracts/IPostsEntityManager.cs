using MyVipCity.DataTransferObjects.Social;
using MyVipCity.Domain.Contracts;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IPostsEntityManager {

		PostDto AddPost<T>(int id, PostDto postDto) where T : class, IPostsEntity;

		PostDto UpdatePost<T>(int id, PostDto postDto) where T : class, IPostsEntity;

		PostDto[] GetPosts<T>(int id, int top) where T : class, IPostsEntity;

		PostDto[] GetPosts<T>(int id, int top, int afterPostId) where T : class, IPostsEntity;
	}
}