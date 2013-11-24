using System;

namespace BigDataSelectorWebClient.Models.BigFileSelector.Result
{
    public class SelectionInProgressResult : BigFileSelectorResult
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