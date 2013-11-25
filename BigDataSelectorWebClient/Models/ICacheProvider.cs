using System;
using System.Collections.Generic;

namespace BigDataSelectorWebClient.Models
{
    public interface ICacheProvider
    {
        bool TryGetSelectedValues(out IEnumerable<string> selectedValues, out TimeSpan calculationTime);
        void CacheResult(IList<string> topStrings, TimeSpan calculationTime);
    }
}