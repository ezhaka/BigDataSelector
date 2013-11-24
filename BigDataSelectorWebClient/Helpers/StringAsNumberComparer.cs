using System.Collections.Generic;

namespace BigDataSelector
{
    public class StringAsNumberComparer : IComparer<string>
    {
        public int Compare(string left, string right)
        {
            if (right.Length > left.Length)
            {
                return -1;
            }

            if (left.Length > right.Length)
            {
                return 1;
            }

            for (int i = 0; i < left.Length; i++)
            {
                char leftChar = left[i];
                char rightChar = right[i];

                if (leftChar > rightChar)
                {
                    return 1;
                }

                if (rightChar > leftChar)
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}