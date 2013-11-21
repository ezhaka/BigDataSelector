using System;
using System.Collections.Generic;
using BigDataSelectorWebClient.Models.Result;

namespace BigDataSelectorWebClient.Models
{
    public class TopElementsProvider
    {
        private ICacheProvider cacheProvider;
        private IBigFileSelector bigFileSelector;

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

            throw new NotImplementedException();
        }
    }
}