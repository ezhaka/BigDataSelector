using System.Collections.Generic;
using BigDataSelectorWebClient.Helpers;

namespace BigDataSelectorWebClient.Models.BigFileSelector
{
    public class TopElementsSelector
    {
        private long itemsProcessed;

        public long ItemsProcessed
        {
            get
            {
                return this.itemsProcessed;
            }
        }

        public IList<int> Select(IEnumerable<string> items, int count)
        {
            PriorityQueue<int> queue = new PriorityQueue<int>(new ReverseIntComparer());
            StringAsNumberComparer stringComparer = new StringAsNumberComparer();
            string maxValueString = "";
            itemsProcessed = 0;

            foreach (var item in items)
            {
                if (itemsProcessed == 0)
                {
                    maxValueString = item;
                }

                if (itemsProcessed < count)
                {
                    int number = int.Parse(item);

                    if (itemsProcessed > 0 && number > queue.Peek())
                    {
                        maxValueString = number.ToString();
                    }

                    queue.Add(number);

                }
                else if (stringComparer.Compare(maxValueString, item) > 0) // string comparison is much more faster than int.Parse
                {
                    queue.Next();
                    queue.Add(int.Parse(item));

                    int newMaxValue = queue.Peek();
                    maxValueString = newMaxValue.ToString();
                }

                itemsProcessed++;
            }

            List<int> result = queue.ToList();
            result.Sort();

            return result;
        }
    }
}