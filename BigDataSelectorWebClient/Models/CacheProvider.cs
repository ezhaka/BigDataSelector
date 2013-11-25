using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using BigDataSelector;

namespace BigDataSelectorWebClient.Models
{
    public class CacheProvider : ICacheProvider
    {
        private string cachePath = ConfigurationManager.AppSettings["CacheFilePath"];
        private static CacheProvider instance;
        private static object syncRoot = new object();

        public static CacheProvider Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new CacheProvider();
                    }
                }

                return instance;
            }
        }

        private CacheProvider()
        {
        }

        public bool TryGetPage(int pageNumber, out IList<string> result)
        {
            if (!File.Exists(this.cachePath))
            {
                result = null;
                return false;
            }

            IEnumerable<string> line = File.ReadLines(this.cachePath);
            result = line.Skip(pageNumber * 1000).Take(1000).ToList();
            return true;
        }

        public void CacheResult(IList<string> topStrings)
        {
            using (StreamWriter writer = new StreamWriter(this.cachePath))
            {
                foreach (var number in topStrings)
                {
                    writer.WriteLine(number);
                }
            }
        }
    }
}