using System;
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
	}
}