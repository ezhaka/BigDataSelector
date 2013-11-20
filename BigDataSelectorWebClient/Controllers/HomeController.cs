using System.Web.Mvc;

namespace BigDataSelectorWebClient.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectionInProgress()
        {
            return View();
        }

    }
}
