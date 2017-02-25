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

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[Route("")]
		public async Task<IHttpActionResult> AddBusiness(BusinessDto businessDto) {
			return await Task<IHttpActionResult>.Factory.StartNew(() => {

				BusinessManager.Create(businessDto);

				return Ok();
			});
		}
	}
}