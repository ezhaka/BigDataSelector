using System.Web.Mvc;
using BigDataSelectorWebClient.Models;
using BigDataSelectorWebClient.Models.BigFileSelector;
using BigDataSelectorWebClient.Models.TopElementsProvider;
using BigDataSelectorWebClient.Models.TopElementsProvider.Result;

namespace BigDataSelectorWebClient.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int pageNumber = 0)
        {
            IBigFileSelector bigFileSelector = BigFileSelector.Instance;
            ICacheProvider cacheProvider = CacheProvider.Instance;
            TopElementsProvider topElementsProvider = new TopElementsProvider(cacheProvider, bigFileSelector);

            this.ViewBag.TopElementsResult = topElementsProvider.GetPage(pageNumber);
            this.ViewBag.CurrentPageNumber = pageNumber;

            return View();
        }

    }
}
