using System.Collections.Generic;

namespace BigDataSelectorWebClient.Helpers
{
    public class ReversedIntComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return y - x;
        }
    }
}