using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using MyVipCity.DataTransferObjects.Social;
using Ninject;

namespace MyVipCity.Controllers.api {

	[RoutePrefix("api/Business")]
	public class BusinessController: ApiController {

		[Inject]
		public IKernel Kernel
		{
			get; set;
		}

		[Inject]
		public IBusinessManager BusinessManager
		{
			get;
			set;
		}

		[HttpGet]
		[Route("")]
		public async Task<IHttpActionResult> Index() {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var dtos = BusinessManager.GetAllBusiness();
				return Ok(dtos);
			});
		}

		[HttpGet]
		[Route("{id:int}")]
		public async Task<IHttpActionResult> GetBusiness(int id) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var dto = BusinessManager.LoadById(id);
				return Ok(dto);
			});
		}

		[HttpGet]
		[Route("ByFriendlyId/{friendlyId}")]
		public async Task<IHttpActionResult> GetByFriendlyIdBusiness(string friendlyId) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var dto = BusinessManager.LoadByFriendlyId(friendlyId);
				return Ok(dto);
			});
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("")]
		public async Task<IHttpActionResult> AddBusiness(BusinessDto businessDto) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var savedBusinessDto = BusinessManager.Create(businessDto);
				return Ok(savedBusinessDto);
			});
		}

		[HttpPut]
		[Authorize(Roles = "Admin")]
		[Route("")]
		public async Task<IHttpActionResult> UpdateBusiness(BusinessDto businessDto) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var updatedBusinessDto = BusinessManager.Update(businessDto);
				return Ok(updatedBusinessDto);
			});
		}

		[HttpGet]
		[Route("{id:int}/Promoters")]
		public async Task<IHttpActionResult> GetPromoters(int id) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var promoters = BusinessManager.GetPromoters(id);
				return Ok(promoters);
			});
		}


		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("{id:int}/PostComment")]
		public async Task<IHttpActionResult> PostComment(int id, CommentPostDto post) {
			return await AddPost(id, post);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("{id:int}/PostPicture")]
		public async Task<IHttpActionResult> PostPicture(int id, PicturePostDto post) {
			return await AddPost(id, post);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("{id:int}/PostVideo")]
		public async Task<IHttpActionResult> PostVideo(int id, VideoPostDto post) {
			return await AddPost(id, post);
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("Posts/{id:int}/{top:int}")]
		public async Task<IHttpActionResult> GetPosts(int id, int top) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var result = BusinessManager.GetPosts(id, top);
				return Ok(result);
			});
		}

		[HttpGet]
		[AllowAnonymous]
		[Route("Posts/{id:int}/{top:int}/{afterPostId:int}")]
		public async Task<IHttpActionResult> GetPosts(int id, int top, int afterPostId) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				var result = BusinessManager.GetPosts(id, top, afterPostId);
				return Ok(result);
			});
		}

		private async Task<IHttpActionResult> AddPost(int id, PostDto post) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {
				post.PostedBy = User.Identity.GetUserId();
				post = BusinessManager.AddPost(id, post);
				return Ok(post);
			});
		}
	}
}