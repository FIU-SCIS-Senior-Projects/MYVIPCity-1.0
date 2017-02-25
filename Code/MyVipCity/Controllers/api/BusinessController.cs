using System;
using System.Threading.Tasks;
using System.Web.Http;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
using Ninject;

namespace MyVipCity.Controllers.api {

	[RoutePrefix("api/Business")]
	public class BusinessController: ApiController {

		[Inject]
		public IBusinessManager BusinessManager
		{
			get;
			set;
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
	}
}