using System;
using System.Collections.Generic;

namespace BigDataSelectorWebClient.Models.TopElementsProvider.Result
{
    public class PageResult : TopElementsProviderResult
    {
        public PageResult(IEnumerable<string> page, TimeSpan calculationTime, int pagesCount)
        {
            Page = page;
            CalculationTime = calculationTime;
            PagesCount = pagesCount;
        }

        public IEnumerable<string> Page { get; set; }
        public TimeSpan CalculationTime { get; set; }
        public int PagesCount { get; set; }
    }
}