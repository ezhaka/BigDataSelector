using System;
using System.Collections.Generic;

namespace BigDataSelectorWebClient.Models.TopElementsProvider.Result
{
    public class PageResult : TopElementsProviderResult
    {
        public PageResult(IEnumerable<string> page, TimeSpan calculationTime)
        {
            Page = page;
            CalculationTime = calculationTime;
        }

        public IEnumerable<string> Page { get; set; }
        public TimeSpan CalculationTime { get; set; }
    }
}