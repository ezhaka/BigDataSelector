using System.Collections.Generic;

namespace BigDataSelectorWebClient.Helpers
{
    public class ReverseIntComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return y - x;
        }
    }
}