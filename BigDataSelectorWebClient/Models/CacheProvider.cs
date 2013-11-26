using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;

namespace BigDataSelectorWebClient.Models
{
    public class CacheProvider : ICacheProvider
    {
        private string cachePath = ConfigurationManager.AppSettings["CacheFilePath"];
        private static CacheProvider instance;
        private static object syncRoot = new object();

        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

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

        public bool TryGetSelectedValues(out IEnumerable<string> selectedValues, out TimeSpan calculationTime)
        {
            this.cacheLock.EnterReadLock();

            try
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
            finally
            {
                this.cacheLock.ExitReadLock();
            }
        }

        public void CacheResult(IList<string> topStrings, TimeSpan calculationTime)
        {
            this.cacheLock.EnterWriteLock();

            try
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
            finally
            {
                this.cacheLock.ExitWriteLock();
            }
        }
    }
}