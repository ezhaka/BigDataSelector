using System.Collections.Generic;

namespace BigDataSelectorWebClient.Models.Result
{
    public class PageResult : TopElementsProviderResult
    {
        public IList<string> Page { get; set; }
    }
}