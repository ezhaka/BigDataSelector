using System;

namespace BigDataSelectorWebClient.Models.TopElementsProvider.Result
{
    public class SelectionInProgressResult : TopElementsProviderResult
    {
        public SelectionInProgressResult(long itemsProcessed, DateTime startDate)
        {
            ItemsProcessed = itemsProcessed;
            StartDate = startDate;
        }

        public long ItemsProcessed { get; set; }
        public DateTime StartDate { get; set; }
    }
}