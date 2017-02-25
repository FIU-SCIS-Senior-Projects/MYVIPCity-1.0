using System.Web.Mvc;

namespace MyVipCity.Controllers
{
    public class ViewBusinessController : Controller
    {
        // GET: AddBusiness
        public ActionResult Index() {
	        return View("~/Views/AddBusiness/Index.cshtml");
        }
    }
}