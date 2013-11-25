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
        private bool inProgress;
        private bool isDone;
        private DateTime startDate;
        long itemsProcessed = 0;

        private BigFileSelector()
        {
            int bufferSize = 10000;
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

            if (inProgress)
            {
                return new SelectionInProgressResult(itemsProcessed, startDate);
            }

            if (!inProgress && !isDone)
            {
                this.startDate = DateTime.UtcNow;
                this.inProgress = true;
                Task.Factory.StartNew(this.StartSelection);

                return new SelectionInProgressResult(itemsProcessed, startDate);
            }

            if (isDone)
            {
                return new SelectionIsDoneResult(this.buffer.Select(i => i.ToString()).ToList(), DateTime.UtcNow - startDate);
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
            this.inProgress = false;
            this.isDone = true;
        }
    }
}