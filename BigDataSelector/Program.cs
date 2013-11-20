using System;
using System.Collections.Generic;
using System.IO;

namespace BigDataSelector
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime globalStart = DateTime.UtcNow;

            int bufferSize = 10000;
            int CONST_smallChunkSize = 10 * 1024 * 1024; //10Mb

            string path = @"E:\BigFile\bigfile.txt";
            List<int> buffer = new List<int>(bufferSize);

            var lines = File.ReadLines(path);

            int maxValue = 0;
            string maxValueString = "";
            int indexOfMaxValue = 0;
            long index = 0;
            DateTime start = DateTime.UtcNow;
            int swapsCount = 0;

            foreach (var line in lines)
            {
                if (index == 0)
                {
                    int number = int.Parse(line);
                    maxValue = number;
                    maxValueString = maxValue.ToString();
                }

                if (index < bufferSize)
                {
                    int number = int.Parse(line);
                    buffer.Add(number);

                    if (number > maxValue)
                    {
                        maxValue = number;
                        maxValueString = maxValue.ToString();
                        indexOfMaxValue = (int)index;
                    }

                }
                else if (IsLeftGreater(maxValueString, line))
                {
                    buffer[indexOfMaxValue] = int.Parse(line);

                    Tuple<int, int> maxIndexAndValue = GetMaxIndexAndElement(buffer);
                    indexOfMaxValue = maxIndexAndValue.Item1;
                    maxValue = maxIndexAndValue.Item2;
                    maxValueString = maxValue.ToString();

                    swapsCount++;
                }

                index++;

                if (index % CONST_smallChunkSize == 0)
                {
                    Console.WriteLine("{0} numbers passed for {1}ms, swaps count={2}", index, (DateTime.UtcNow - start).TotalMilliseconds, swapsCount);
                    start = DateTime.UtcNow;
                    swapsCount = 0;
                }
            }

            buffer.Sort();

            string firstElementsPath = "firstElements.txt";

            using (StreamWriter writer = new StreamWriter(firstElementsPath))
            {
                foreach (var number in buffer)
                {
                    writer.WriteLine(number);
                }
            }

            Console.WriteLine("Global time: {0}m", (DateTime.UtcNow - globalStart).TotalMinutes);
            Console.ReadKey();
        }

        static int ParseInt(string stringValue)
        {
            int result = 0;

            for (int i = 0; i < stringValue.Length; i++)
            {
                result = result * 10 + (stringValue[i] - '0');
            }

            return result;
        }

        static Tuple<int, int> GetMaxIndexAndElement(IList<int> list)
        {
            int maxIndex = 0;
            int maxValue = list[0];

            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] > maxValue)
                {
                    maxValue = list[i];
                    maxIndex = i;
                }
            }

            return new Tuple<int, int>(maxIndex, maxValue);
        }

        static bool IsLeftGreater(string left, string right)
        {
            if (right.Length > left.Length)
            {
                return false;
            }

            if (left.Length > right.Length)
            {
                return true;
            }

            for (int i = 0; i < left.Length; i++)
            {
                char leftChar = left[i];
                char rightChar = right[i];

                if (leftChar > rightChar)
                {
                    return true;
                }

                if (rightChar > leftChar)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
