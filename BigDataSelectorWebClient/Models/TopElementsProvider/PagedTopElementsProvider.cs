using System;
using System.Collections.Generic;
using BigDataSelector;
using BigDataSelectorWebClient.Models.BigFileSelector;
using BigDataSelectorWebClient.Models.TopElementsProvider.Result;

namespace BigDataSelectorWebClient.Models.TopElementsProvider
{
    public class PagedTopElementsProvider
    {
        private readonly IBigFileSelector bigFileSelector;

        public PagedTopElementsProvider(IBigFileSelector bigFileSelector)
        {
            this.bigFileSelector = bigFileSelector;
        }

        public TopElementsProviderResult GetPage(int pageNumber)
        {
            if (pageNumber < 1)
            {
                return new InvalidPageNumberResult();
            }

            var bigFileSelectorResult = this.bigFileSelector.SelectTopElements();

            if (bigFileSelectorResult is BigFileSelector.Result.FileNotFoundResult)
            {
                return new FileNotFoundResult();
            }

            if (bigFileSelectorResult is BigFileSelector.Result.SelectionInProgressResult)
            {
                var selectionInProgressResult = (BigFileSelector.Result.SelectionInProgressResult) bigFileSelectorResult;

                return new SelectionInProgressResult(
                    selectionInProgressResult.ItemsProcessed,
                    selectionInProgressResult.StartDate);
            }

            if (bigFileSelectorResult is BigFileSelector.Result.SelectionIsDoneResult)
            {
                var selectionIsDoneResult = (BigFileSelector.Result.SelectionIsDoneResult) bigFileSelectorResult;
                
                return this.GetPageResult(
                    selectionIsDoneResult.SelectedValues, 
                    selectionIsDoneResult.CalculationTime, 
                    pageNumber);
            }

            throw new Exception("Unknown BigFileSelectorManager result");
        }

        private TopElementsProviderResult GetPageResult(IEnumerable<string> selectedValues, TimeSpan calculationTime, int pageNumber)
        {
            int pagesCount = PageManager.GetPagesCount(selectedValues);

            if (pagesCount < pageNumber)
            {
                return new InvalidPageNumberResult();
            }

            return new PageResult(PageManager.GetPage(selectedValues, pageNumber), calculationTime);
        }
    }
}