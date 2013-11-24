using System.Collections.Generic;

namespace BigDataSelectorWebClient.Models
{
    public interface ICacheProvider
    {
        bool TryGetPage(int pageNumber, out IList<string> result);
        void CacheResult(IList<string> topStrings);
    }
}