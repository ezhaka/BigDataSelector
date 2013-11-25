using System.Collections.Generic;
using System.Linq;

namespace BigDataSelector
{
    public class PageManager
    {
        private const int itemsPerPage = 1000;

        public static int GetPagesCount(IEnumerable<string> values)
        {
            return values.Count() % itemsPerPage;
        }

        public static IList<string> GetPage(IEnumerable<string> values, int pageNumber)
        {
            return values.Skip((pageNumber - 1) * itemsPerPage).Take(itemsPerPage).ToList();
        }
    }
}