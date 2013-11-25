using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BigDataSelector;
using BigDataSelectorWebClient.Models.BigFileSelector.Result;

namespace BigDataSelectorWebClient.Models.BigFileSelector
{
    public class BigFileSelector : IBigFileSelector
    {
        private static BigFileSelector instance;
        private static object syncRoot = new object();

        string path = ConfigurationManager.AppSettings["BigFilePath"];
        List<int> buffer;
        private int bufferSize = 10000;
        private SelectorState state = SelectorState.NotStarted;
        private DateTime startDate;
        private TimeSpan elapsedTime;
        long itemsProcessed = 0;
        private object syncSelectionStart = new object();

        private BigFileSelector()
        {
            buffer = new List<int>(bufferSize);
        }

        public static BigFileSelector Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new BigFileSelector();
                    }
                }

                return instance;
            }
        }

        public BigFileSelectorResult SelectTopElements()
        {
            if (!File.Exists(path))
            {
                return new FileNotFoundResult();
            }

            lock (syncSelectionStart)
            {
                if (state == SelectorState.NotStarted)
                {
                    this.startDate = DateTime.UtcNow;
                    state = SelectorState.InProgress;
                    Task.Factory.StartNew(this.StartSelection);

                    return new SelectionInProgressResult(itemsProcessed, startDate);
                }
            }

            if (state == SelectorState.Done)
            {
                return new SelectionIsDoneResult(this.buffer.Select(i => i.ToString()).ToList(), this.elapsedTime);
            }

            if (state == SelectorState.InProgress)
            {
                return new SelectionInProgressResult(itemsProcessed, startDate);
            }

            throw new Exception("Unknown BigFileSelectorState");
        }

        private void StartSelection()
        {
            IEnumerable<string> lines = File.ReadLines(path);
            PriorityQueue<int> queue = new PriorityQueue<int>(new ReverseIntComparer());
            StringAsNumberComparer stringComparer = new StringAsNumberComparer();
            string maxValueString = "";

            foreach (var line in lines)
            {
                if (itemsProcessed == 0)
                {
                    maxValueString = line;
                }

                if (itemsProcessed < bufferSize)
                {
                    int number = int.Parse(line);

                    if (itemsProcessed > 0 && number > queue.Peek())
                    {
                        maxValueString = number.ToString();
                    }

                    queue.Add(number);

                }
                else if (stringComparer.Compare(maxValueString, line) == 1)
                {
                    queue.Next();
                    queue.Add(int.Parse(line));

                    int newMaxValue = queue.Peek();
                    maxValueString = newMaxValue.ToString();
                }

                itemsProcessed++;
            }


            buffer = queue.ToList();
            buffer.Sort();
            this.elapsedTime = DateTime.UtcNow - this.startDate;
            this.state = SelectorState.Done;
        }
    }
}