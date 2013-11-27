using System;
using System.Web.Mvc;
using BigDataSelector;
using BigDataSelectorWebClient.Models.BigFileSelector;
using BigDataSelectorWebClient.Models.BigFileSelector.Result;

namespace BigDataSelectorWebClient.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(int pageNumber = 1)
        {
            IBigFileSelector bigFileSelector = BigFileSelectorManager.Instance;


            if (pageNumber < 1)
            {
                return View("InvalidPageNumber");
            }

            var bigFileSelectorResult = bigFileSelector.SelectTopElements();

            if (bigFileSelectorResult is FileNotFoundResult)
            {
                return View("FileNotFound");
            }

            if (bigFileSelectorResult is SelectionInProgressResult)
            {
                var selectionInProgressResult = (SelectionInProgressResult)bigFileSelectorResult;

                this.ViewBag.ItemsProcessed = selectionInProgressResult.ItemsProcessed;
                this.ViewBag.StartDate = selectionInProgressResult.StartDate;

                return View("SelectionInProgress");
            }

            if (bigFileSelectorResult is SelectionIsDoneResult)
            {
                var selectionIsDoneResult = (SelectionIsDoneResult)bigFileSelectorResult;
                int pagesCount = PageManager.GetPagesCount(selectionIsDoneResult.SelectedValues);

                if (pagesCount < pageNumber)
                {
                    return View("InvalidPageNumber");
                }

                this.ViewBag.CalculationTime = selectionIsDoneResult.CalculationTime;
                this.ViewBag.CurrentPageNumber = pageNumber;
                this.ViewBag.PagesCount = pagesCount;
                this.ViewBag.SelectedValues = PageManager.GetPage(selectionIsDoneResult.SelectedValues, pageNumber);
                
                return View("Page");
            }

            throw new Exception("Unknown BigFileSelectorResult");
        }

    }
}
