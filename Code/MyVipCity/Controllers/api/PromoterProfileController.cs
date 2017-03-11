using System.Threading.Tasks;
using System.Web.Http;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
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
	}
}