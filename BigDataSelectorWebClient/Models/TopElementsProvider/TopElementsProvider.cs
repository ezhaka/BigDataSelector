using System;
using System.Collections.Generic;
using BigDataSelector;
using BigDataSelectorWebClient.Models.BigFileSelector;
using BigDataSelectorWebClient.Models.TopElementsProvider.Result;

namespace BigDataSelectorWebClient.Models.TopElementsProvider
{
    public class TopElementsProvider
    {
        private readonly ICacheProvider cacheProvider;
        private readonly IBigFileSelector bigFileSelector;

        public TopElementsProvider(ICacheProvider cacheProvider, IBigFileSelector bigFileSelector)
        {
            this.cacheProvider = cacheProvider;
            this.bigFileSelector = bigFileSelector;
        }

        public TopElementsProviderResult GetPage(int pageNumber)
        {
            if (pageNumber < 1)
            {
                return new InvalidPageNumberResult();
            }

            IEnumerable<string> selectedValues;
            TimeSpan calculationTime;

            if (this.cacheProvider.TryGetSelectedValues(out selectedValues, out calculationTime))
            {
                return this.GetPageResult(selectedValues, calculationTime, pageNumber);
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
                this.cacheProvider.CacheResult(selectionIsDoneResult.SelectedValues, selectionIsDoneResult.CalculationTime);

                return new PageResult(selectionIsDoneResult.SelectedValues, selectionIsDoneResult.CalculationTime);
            }

            throw new Exception("Unknown BigFileSelector result");
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