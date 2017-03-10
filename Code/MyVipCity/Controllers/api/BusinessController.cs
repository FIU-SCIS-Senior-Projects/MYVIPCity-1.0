using System;
using System.Data.Entity.Migrations.Model;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Web.Http;
using MyVipCity.BusinessLogic.Contracts;
using MyVipCity.DataTransferObjects;
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
	}
}