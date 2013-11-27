using System;
using System.Collections;
using System.Collections.Generic;
using BigDataSelectorWebClient.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigFileSelectorTests
{
    [TestClass]
    public class PriorityQueueTest
    {
        [TestMethod]
        public void ManualRandomValues()
        {
            PriorityQueue<int> priorityQueue = new PriorityQueue<int>(Comparer<int>.Default);

            priorityQueue.Add(-6);
            priorityQueue.Add(6);
            priorityQueue.Add(-7);
            priorityQueue.Add(-9);
            priorityQueue.Add(-8);

            Assert.AreEqual(-9, priorityQueue.Next());
            Assert.AreEqual(-8, priorityQueue.Next());
            Assert.AreEqual(-7, priorityQueue.Next());
            Assert.AreEqual(-6, priorityQueue.Next());
            Assert.AreEqual(6, priorityQueue.Next());
        }

        [TestMethod]
        public void RandomValues()
        {
            PriorityQueue<int> priorityQueue = new PriorityQueue<int>(Comparer<int>.Default);

            Random random = new Random();
            List<int> expected = new List<int>();
            var count = 1000;

            for (int i = 0; i < count; i++)
            {
                var value = random.Next();

                priorityQueue.Add(value);
                expected.Add(value);
            }

            expected.Sort();

            for (int i = 0; i < count; i++)
            {
                Assert.AreEqual(expected[i], priorityQueue.Next());
            }
        }
    }
}
