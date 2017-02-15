using System.Web.Mvc;

namespace MyVipCity.Controllers
{
    public class ExampleController : Controller
    {
        // GET: Example
        public ActionResult Index()
        {
			ViewBag.HideNgView = true;
			return View();
        }
    }
}