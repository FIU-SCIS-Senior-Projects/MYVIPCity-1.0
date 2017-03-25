using MyVipCity.DataTransferObjects;
using MyVipCity.DataTransferObjects.Social;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IPromoterProfileManager {

		PromoterProfileDto[] GetPromoterProfiles(string userId);

		PromoterProfileDto GetProfileById(int id);

		PromoterProfileDto Update(PromoterProfileDto promoterProfileDto);

		string GetPromoterEmail(int id);

		void Delete(int id);

		ResultDto<bool> AddReview(int id, ReviewDto review);

		ResultDto<bool> RemoveReview(int id);

		ReviewDto[] GetReviews(int id, int top);

		ReviewDto[] GetReviews(int id, int top, int afterReviewId);

		PostDto AddOrUpdatePost(int id, PostDto postDto);

		ResultDto<bool> DeletePost(int id, int postId);

		PostDto[] GetPosts(int id, int top);

		PostDto[] GetPosts(int id, int top, int afterPostId);
	}
}