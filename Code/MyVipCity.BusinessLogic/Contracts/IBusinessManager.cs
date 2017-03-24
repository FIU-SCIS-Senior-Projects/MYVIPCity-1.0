using MyVipCity.DataTransferObjects;
using MyVipCity.DataTransferObjects.Social;

namespace MyVipCity.BusinessLogic.Contracts {

	public interface IBusinessManager {

		BusinessDto Create(BusinessDto businessDto);

		BusinessDto Update(BusinessDto businessDto);

		BusinessDto LoadById(int id);

		BusinessDto LoadByFriendlyId(string friendlyId);

		BusinessDto[] GetAllBusiness();

		PromoterProfileDto[] GetPromoters(int id);

		PostDto AddOrUpdatePost(int id, PostDto post);

		ResultDto<bool> DeletePost(int id, int postId);

		PostDto[] GetPosts(int id, int top);

		PostDto[] GetPosts(int id, int top, int afterPostId);
	}
}