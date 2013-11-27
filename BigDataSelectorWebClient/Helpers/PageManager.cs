using System;
using System.Collections.Generic;
using System.Linq;

namespace BigDataSelectorWebClient.Helpers
{
    public class PageManager
    {
        private const int itemsPerPage = 1000;

        public static int GetPagesCount(IEnumerable<string> values)
        {
            return (int)Math.Ceiling((decimal)values.Count() / itemsPerPage);
        }

        public static IList<string> GetPage(IEnumerable<string> values, int pageNumber)
        {
            return values.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
        }
    }
}