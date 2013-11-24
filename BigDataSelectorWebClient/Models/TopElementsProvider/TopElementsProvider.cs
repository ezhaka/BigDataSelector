using System;
using System.Collections.Generic;
using System.Linq;
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
            IList<string> page;

            if (this.cacheProvider.TryGetPage(pageNumber, out page))
            {
                return new PageResult
                {
                    Page = page
                };
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

                this.cacheProvider.CacheResult(selectionIsDoneResult.TopStrings);
                const int itemsPerPage = 1000;

                return new PageResult
                {
                    Page = selectionIsDoneResult.TopStrings.Skip(pageNumber * itemsPerPage).Take(itemsPerPage).ToList()
                };
            }

            throw new Exception("Unknown BigFileSelector result");
        }
    }
}