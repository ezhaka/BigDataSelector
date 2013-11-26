using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace BigDataSelectorWebClient.Models
{
    public class CacheProvider : ICacheProvider
    {
        private string cachePath = ConfigurationManager.AppSettings["CacheFilePath"];

        public bool TryGetSelectedValues(out IEnumerable<string> selectedValues, out TimeSpan calculationTime)
        {
            if (!File.Exists(this.cachePath))
            {
                selectedValues = null;
                calculationTime = default(TimeSpan);
                return false;
            }
            
            IEnumerable<string> lines = File.ReadLines(this.cachePath);
            calculationTime = TimeSpan.Parse(lines.First());
            selectedValues = lines.Skip(1);
            return true;
        }

        public void CacheResult(IList<string> topStrings, TimeSpan calculationTime)
        {
            using (StreamWriter writer = new StreamWriter(this.cachePath))
            {
                writer.WriteLine(calculationTime);

                foreach (var value in topStrings)
                {
                    writer.WriteLine(value);
                }
            }
        }
    }
}