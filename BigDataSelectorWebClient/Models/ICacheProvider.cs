using System.Collections.Generic;

namespace BigDataSelectorWebClient.Models
{
    public interface ICacheProvider
    {
        bool TryGetPage(out IList<string> result);
    }
}