using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using MyVipCity.Mailing.Contracts;

namespace MyVipCity.Controllers {

	[RoutePrefix("api/Values")]
	public class ValuesController: ApiController {
		public ValuesController(IEmailService emailService) {

		}

		[HttpGet]
		[Route("")]
		public IQueryable<int> Values() {
			var list = new List<int>();
			list.Add(1);

			return list.AsQueryable();
		}
	}
}
