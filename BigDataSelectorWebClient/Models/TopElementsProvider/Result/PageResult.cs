using System.Collections.Generic;

namespace BigDataSelectorWebClient.Models.TopElementsProvider.Result
{
    public class PageResult : TopElementsProviderResult
    {
        public IList<string> Page { get; set; }
    }
}