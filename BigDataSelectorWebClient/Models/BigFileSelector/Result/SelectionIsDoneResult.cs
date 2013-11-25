using System;
using System.Collections.Generic;

namespace BigDataSelectorWebClient.Models.BigFileSelector.Result
{
    public class SelectionIsDoneResult : BigFileSelectorResult
    {
        public SelectionIsDoneResult(IList<string> selectedValues, TimeSpan calculationTime)
        {
            SelectedValues = selectedValues;
            CalculationTime = calculationTime;
        }

        public IList<string> SelectedValues { get; set; }
        public TimeSpan CalculationTime { get; set; }
    }
}