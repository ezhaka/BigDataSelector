using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BigDataSelectorWebClient.Models.BigFileSelector.Result;

namespace BigDataSelectorWebClient.Models.BigFileSelector
{
    public class BigFileSelectorManager : IBigFileSelector
    {
        private static BigFileSelectorManager instance;
        private static object syncRoot = new object();

        private ReaderWriterLockSlim doneSelectionLock = new ReaderWriterLockSlim();
        private object syncSelectionStart = new object();

        string path = ConfigurationManager.AppSettings["BigFilePath"];
        private int bufferSize = 10000;
        private SelectorState state = SelectorState.Idle;
        private DateTime startDate;
        private TopElementsSelector selector;

        private BigFileSelectorManager()
        {
        }

        public static BigFileSelectorManager Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new BigFileSelectorManager();
                    }
                }

                return instance;
            }
        }

        public BigFileSelectorResult SelectTopElements()
        {
            this.doneSelectionLock.EnterReadLock();

            try
            {
                IEnumerable<string> selectedValues;
                TimeSpan calculationTime;
                ICacheProvider cacheProvider = new CacheProvider();

                if (cacheProvider.TryGetSelectedValues(out selectedValues, out calculationTime))
                {
                    return new SelectionIsDoneResult(selectedValues, calculationTime);
                }

                if (!File.Exists(path))
                {
                    return new FileNotFoundResult();
                }

                lock (syncSelectionStart)
                {
                    if (state == SelectorState.Idle)
                    {
                        this.startDate = DateTime.UtcNow;
                        state = SelectorState.InProgress;

                        this.selector = new TopElementsSelector();
                        Task.Factory.StartNew(this.StartSelection);

                        return new SelectionInProgressResult(this.selector.ItemsProcessed, startDate);
                    }
                }

                if (state == SelectorState.InProgress)
                {
                    return new SelectionInProgressResult(this.selector.ItemsProcessed, startDate);
                }

                throw new Exception("Unknown BigFileSelectorState");
            }
            finally
            {
                this.doneSelectionLock.ExitReadLock();
            }
        }

        private void StartSelection()
        {
            IEnumerable<string> lines = File.ReadLines(path);
            IList<int> result = this.selector.Select(lines, bufferSize);

            this.doneSelectionLock.EnterWriteLock();

            try
            {
                ICacheProvider cacheProvider = new CacheProvider();
                cacheProvider.CacheResult(result.Select(i => i.ToString()).ToList(), DateTime.UtcNow - this.startDate);

                this.state = SelectorState.Idle;
                this.selector = null;
            }
            finally
            {
                this.doneSelectionLock.ExitWriteLock();
            }
        }
    }
}