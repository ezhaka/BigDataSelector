using System.Collections.Generic;

namespace BigDataSelectorWebClient.Models
{
    public interface IDataProvider
    {
        bool IsFileExists();
        IEnumerable<string> GetData();
    }
}