using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace BigDataSelectorWebClient.Models
{
    public class DataProvider : IDataProvider
    {
        readonly string path = ConfigurationManager.AppSettings["BigFilePath"];

        public bool IsFileExists()
        {
            return File.Exists(path);
        }

        public IEnumerable<string> GetData()
        {
            return File.ReadLines(path);
        }
    }
}