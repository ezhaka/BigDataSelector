using System.Collections.Generic;

namespace BigDataSelectorWebClient.Models.BigFileSelector.Result
{
    public class SelectionIsDoneResult : BigFileSelectorResult
    {
        public SelectionIsDoneResult(IList<string> topStrings)
        {
            TopStrings = topStrings;
        }

        public IList<string> TopStrings { get; set; }
    }
}