using System.Collections.Generic;

namespace BigDataSelector
{
    public class StringAsNumberComparer : IComparer<string>
    {
        public int Compare(string left, string right)
        {
            if (left[0] == '-' && right[0] != '-')
            {
                return -1;
            }

            if (left[0] != '-' && right[0] == '-')
            {
                return 1;
            }

            bool bothNegative = left[0] == '-' && right[0] == '-';

            if (right.Length > left.Length)
            {
                return bothNegative  ? 1 : - 1;
            }

            if (left.Length > right.Length)
            {
                return bothNegative ? -1 : 1;
            }

            for (int i = 0; i < left.Length; i++)
            {
                char leftChar = left[i];
                char rightChar = right[i];

                if (leftChar > rightChar)
                {
                    return bothNegative ? -1 : 1;
                }

                if (rightChar > leftChar)
                {
                    return bothNegative ? 1 : -1;
                }
            }

            return 0;
        }
    }
}