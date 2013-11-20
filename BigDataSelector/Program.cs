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

            string path = @"C:\Users\mcgee\Downloads\BigFilesGenerator\BigFilesGenerator\BigFilesGenerator\bin\Debug\bigfile.txt";
            List<int> buffer = new List<int>(bufferSize);
            PriorityQueue<int> queue = new PriorityQueue<int>(new ReverseIntComparer());

            var lines = File.ReadLines(path);

            string maxValueString = "";
            long index = 0;
            DateTime start = DateTime.UtcNow;
            int swapsCount = 0;

            foreach (var line in lines)
            {
                if (index == 0)
                {
                    maxValueString = line;
                }

                if (index < bufferSize)
                {
                    int number = int.Parse(line);

                    if (index > 0 && number > queue.Peek())
                    {
                        maxValueString = number.ToString();
                    }

                    queue.Add(number);

                }
                else if (IsLeftGreater(maxValueString, line))
                {
                    queue.Next();
                    queue.Add(int.Parse(line));

                    int newMaxValue = queue.Peek();
                    maxValueString = newMaxValue.ToString();

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


            buffer = queue.ToList();
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
