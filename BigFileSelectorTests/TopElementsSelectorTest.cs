using System;
using System.Collections.Generic;
using System.Linq;
using BigDataSelectorWebClient.Models.BigFileSelector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BigFileSelectorTests
{
    [TestClass]
    public class TopElementsSelectorTest
    {
        [TestMethod]
        public void ReversedSource()
        {
            var selector = new TopElementsSelector();
            var testValues = (new List<int> { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1 }).Select(i => i.ToString());

            IList<int> result = selector.Select(testValues, 5);
            Assert.AreEqual(5, result.Count);

            for (int i = 0; i < result.Count(); i++)
            {
                Assert.AreEqual(i + 1, result[i]);
            }
        }

        [TestMethod]
        public void RandomValues()
        {
            var selector = new TopElementsSelector();
            Random random = new Random();

            IList<string> source = Enumerable.Range(0, 10000).Select(i => random.Next().ToString()).ToList();

            int selectionCount = 1000;
            IList<int> result = selector.Select(source, selectionCount);

            Assert.AreEqual(result.Count, selectionCount);
            List<int> expectedValues = source.Select(int.Parse).ToList();
            expectedValues.Sort();
            expectedValues = expectedValues.Take(1000).ToList();

            for (var i = 0; i < selectionCount; i++)
            {
                Assert.AreEqual(expectedValues[i], result[i]);
            }
        }

        [TestMethod]
        public void NegativeValues() // negative values is not supported yet
        {
            var selector = new TopElementsSelector();
            var testValues = (new List<int> { -10, -9, -8, 7, 6, 5, 4, -3, 2, 1 }).Select(i => i.ToString());

            IList<int> result = selector.Select(testValues, 5);
            Assert.AreEqual(5, result.Count);

            Assert.AreEqual(-10, result[0]);
            Assert.AreEqual(-9, result[1]);
            Assert.AreEqual(-8, result[2]);
            Assert.AreEqual(-3, result[3]);
            Assert.AreEqual(0, result[4]);
        }
    }
}
