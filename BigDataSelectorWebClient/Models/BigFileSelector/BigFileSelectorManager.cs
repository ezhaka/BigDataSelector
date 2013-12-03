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
    public class BigFileSelectorManager : IBigFileSelectorManager
    {
        private static BigFileSelectorManager instance;
        private static readonly object syncRoot = new object();

        private readonly ReaderWriterLockSlim doneSelectionLock = new ReaderWriterLockSlim();
        private readonly object syncSelectionStart = new object();

        private const int bufferSize = 10000;
        private SelectorManagerState state = SelectorManagerState.Idle;
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

                IDataProvider dataProvider = new DataProvider();

                if (!dataProvider.IsFileExists())
                {
                    return new FileNotFoundResult();
                }

                lock (syncSelectionStart)
                {
                    if (state == SelectorManagerState.Idle)
                    {
                        this.startDate = DateTime.UtcNow;
                        state = SelectorManagerState.InProgress;

                        this.selector = new TopElementsSelector();
                        IEnumerable<string> data = dataProvider.GetData();
                        Task.Factory.StartNew(() => this.StartSelection(data));

                        return new SelectionInProgressResult(this.selector.ItemsProcessed, startDate);
                    }
                }

                if (state == SelectorManagerState.InProgress)
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

        private void StartSelection(IEnumerable<string> lines)
        {
            IList<int> result = this.selector.Select(lines, bufferSize);

            this.doneSelectionLock.EnterWriteLock();

            try
            {
                ICacheProvider cacheProvider = new CacheProvider();
                cacheProvider.CacheResult(result.Select(i => i.ToString()).ToList(), DateTime.UtcNow - this.startDate);

                this.state = SelectorManagerState.Idle;
                this.selector = null;
            }
            finally
            {
                this.doneSelectionLock.ExitWriteLock();
            }
        }
    }
}