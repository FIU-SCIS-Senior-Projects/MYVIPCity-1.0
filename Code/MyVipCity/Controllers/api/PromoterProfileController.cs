using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using MyVipCity.DataTransferObjects.Social;
using Ninject;

namespace MyVipCity.Controllers.api {

	[RoutePrefix("api/PromoterProfile")]
	public class PromoterProfileController: ApiController {

		[Inject]
		public IKernel Kernel
		{
			get; set;
		}

		[Inject]
		public IPromoterProfileManager PromoterProfileManager
		{
			get;
			set;
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("{id:int}")]
		public async Task<IHttpActionResult> GetProfileById(int id) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var result = PromoterProfileManager.GetProfileById(id);
				return Ok(result);
			});
		}

		[HttpPut]
		[Authorize(Roles = "Promoter")]
		[Route("")]
		public async Task<IHttpActionResult> UpdatePromoterProfile(PromoterProfileDto promoterProfileDto) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var updatedPromoterProfileDto = PromoterProfileManager.Update(promoterProfileDto);
				return Ok(updatedPromoterProfileDto);
			});
		}

		[HttpDelete]
		[Authorize(Roles = "Admin")]
		[Route("{id:int}")]
		public async Task<IHttpActionResult> DeletePromoterProfile(int id) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				PromoterProfileManager.Delete(id);
				return Ok();
			});
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		[Route("{id:int}/Email")]
		public async Task<IHttpActionResult> GetPromoterEmail(int id) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var email = PromoterProfileManager.GetPromoterEmail(id);
				return Ok(email);
			});
		}

		[HttpPost]
		[Authorize]
		[Route("Review/{id:int}")]
		public async Task<IHttpActionResult> AddReview(int id, ReviewDto review) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				review.ReviewerEmail = User.Identity.Name;
				var result = PromoterProfileManager.AddReview(id, review);
				return Ok(result);
			});
		}

		[HttpDelete]
		[Authorize(Roles = "Admin")]
		[Route("Review/{id:int}")]
		public async Task<IHttpActionResult> RemoveReview(int id) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var result = PromoterProfileManager.RemoveReview(id);
				return Ok(result);
			});
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("Reviews/{id:int}/{top:int}")]
		public async Task<IHttpActionResult> GetReviews(int id, int top) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var reviews = PromoterProfileManager.GetReviews(id, top);
				return Ok(reviews);
			});
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("Reviews/{id:int}/{top:int}/{afterReviewId:int}")]
		public async Task<IHttpActionResult> GetReviews(int id, int top, int afterReviewId) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var reviews = PromoterProfileManager.GetReviews(id, top, afterReviewId);
				return Ok(reviews);
			});
		}

		[HttpPost]
		[Authorize(Roles = "Promoter")]
		[Route("{id:int}/PostComment")]
		public async Task<IHttpActionResult> PostComment(int id, CommentPostDto post) {
			return await AddOrUpdatePost(id, post);
		}

		[HttpPost]
		[Authorize(Roles = "Promoter")]
		[Route("{id:int}/PostPicture")]
		public async Task<IHttpActionResult> PostPicture(int id, PicturePostDto post) {
			return await AddOrUpdatePost(id, post);
		}

		[HttpPost]
		[Authorize(Roles = "Promoter")]
		[Route("{id:int}/PostVideo")]
		public async Task<IHttpActionResult> PostVideo(int id, VideoPostDto post) {
			return await AddOrUpdatePost(id, post);
		}

		[HttpDelete]
		[Authorize(Roles = "Promoter")]
		[Route("{id:int}/DeletePost/{postId:int}")]
		public async Task<IHttpActionResult> DeletePost(int id, int postId) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var result = PromoterProfileManager.DeletePost(id, postId);
				return Ok(result);
			});
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("Posts/{id:int}/{top:int}")]
		public async Task<IHttpActionResult> GetPosts(int id, int top) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var result = PromoterProfileManager.GetPosts(id, top);
				return Ok(result);
			});
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("Posts/{id:int}/{top:int}/{afterPostId:int}")]
		public async Task<IHttpActionResult> GetPosts(int id, int top, int afterPostId) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var result = PromoterProfileManager.GetPosts(id, top, afterPostId);
				return Ok(result);
			});
		}

		private async Task<IHttpActionResult> AddOrUpdatePost(int id, PostDto post) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				post.PostedBy = User.Identity.GetUserId();
				post = PromoterProfileManager.AddOrUpdatePost(id, post);
				return Ok(post);
			});
		}
	}
}