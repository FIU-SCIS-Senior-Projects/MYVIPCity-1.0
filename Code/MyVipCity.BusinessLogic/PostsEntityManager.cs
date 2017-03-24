using System;
using System.Linq;
using System.Linq.Expressions;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects.Social;
using MyVipCity.Domain.Contracts;
using MyVipCity.Domain.Social;
using Ninject;
using Ninject.Extensions.Logging;

namespace MyVipCity.BusinessLogic {

	public class PostsEntityManager: AbstractEntityManager, IPostsEntityManager {

		[Inject]
		public ILogger Logger
		{
			get;
			set;
		}

		public PostDto AddPost<T>(int id, PostDto postDto) where T : class, IPostsEntity {
			// make sure this is a new post
			if (postDto.Id != 0) {
				var msg = $"[AddPost] Expected post with Id = 0. Received post with Id={postDto.Id}";
				Logger.Error(msg);
				throw new InvalidOperationException(msg);
			}
			// find the entity with the given id
			var entity = DbContext.Set<T>().Find(id);
			// check if the entity does not exist
			if (entity == null) {
				var msg = $"[AddPost] Entity with Id={id} not found";
				Logger.Error(msg);
				throw new InvalidOperationException(msg);
			}
			// map from dto to model
			var post = Mapper.Map<Post>(postDto);
			// update the posted on time
			post.PostedOn = DateTimeOffset.UtcNow;
			// add the post to the collection of posts
			entity.Posts.Add(post);
			// save changes
			DbContext.SaveChanges();
			postDto = ToDto<PostDto, Post>(post);
			// return success
			return postDto;
		}

		public PostDto UpdatePost<T>(int id, PostDto postDto) where T : class, IPostsEntity {
			// check the post has a valid id
			if (postDto.Id == 0) {
				var msg = $"[UpdatePost] Expected post with Id <> 0. Received post with Id=0";
				Logger.Error(msg);
				throw new InvalidOperationException(msg);
			}
			// find the entity with the given id
			var entity = DbContext.Set<T>().Find(id);
			// check if the entity does not exist
			if (entity == null) {
				var msg = $"[UpdatePost] Entity with Id={id} not found";
				Logger.Error(msg);
				throw new InvalidOperationException(msg);
			}

			// find the post
			var existingPost = entity.Posts.AsQueryable().FirstOrDefault(p => p.Id == postDto.Id);
			if (existingPost == null) {
				var msg = $"[UpdatePost] Post with Id={postDto.Id} not found for entity with Id={id}";
				Logger.Error(msg);
				throw new InvalidOperationException(msg);
			}
			var updatedModel = ToModel(postDto, existingPost);
			DbContext.SaveChanges();
			postDto = ToDto<PostDto, Post>(updatedModel);
			return postDto;
		}

		public PostDto[] GetPosts<T>(int id, int top) where T : class, IPostsEntity {
			return GetPosts<T>(id, top, null);
		}

		public PostDto[] GetPosts<T>(int id, int top, int afterPostId) where T : class, IPostsEntity {
			return GetPosts<T>(id, top, p => p.Id < afterPostId);
		}

		private PostDto[] GetPosts<T>(int id, int top, Expression<Func<Post, bool>> whereExpression) where T : class, IPostsEntity {
			// find the entity
			var entity = DbContext.Set<T>().Find(id);
			// if the entity does not exists, then return null
			if (entity == null)
				return null;
			// get the posts
			IQueryable<Post> postsQueryable = entity.Posts.AsQueryable();
			// if there is a filter, apply it
			if (whereExpression != null)
				postsQueryable = postsQueryable.Where(whereExpression);
			// sort descending by Id (this is the same as ordering descending by PostedOn)
			var posts = postsQueryable.OrderByDescending(r => r.Id).Take(top).ToArray();
			// map to dto
			var postsDto = Mapper.Map<PostDto[]>(posts);
			return postsDto;
		}
	}
}
