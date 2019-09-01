using NUnit.Framework;
using System.Collections.Generic;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class UnionTests
    {
        static readonly List<object[]> UnionIntervalSource = new List<object[]> {
            new object[] { 2, 5, 7, 15, 8, true },
            new object[] { 2, 5, 7, 15, 6, false },
            new object[] { 2, 5, 3, 15, 8, true },
            new object[] { 2, 5, 2, 15, 23, false },
        };

        [TestCaseSource(nameof(UnionIntervalSource))]
        public void Contains_ShouldReturnCorrectResult(
            int lowerFirst,
            int upperFirst,
            int lowerSecond,
            int upperSecond,
            int value,
            bool expected)
        {
            var pairs = new EndPointPair<int>[] {
                new EndPointPair<int>(lowerFirst, upperFirst),
                new EndPointPair<int>(lowerSecond, upperSecond)
            };

            var interval = new Interval<int>(pairs);

            Assert.That(interval.Contains(value), Is.EqualTo(expected));
        }

        static readonly List<object[]> UnionIntervalStringsSource = new List<object[]> {
            new object[] { new List<EndPointPair<int>>() { new EndPointPair<int>(2, 5), new EndPointPair<int>(2, 5) }, "[2, 5]" },
            new object[] { new List<EndPointPair<int>>() { new EndPointPair<int>(2, 5), new EndPointPair<int>(7, 8) }, "[2, 5][7, 8]" },
            new object[] { new List<EndPointPair<int>>() { new EndPointPair<int>(2, 5), new EndPointPair<int>(4, 8) }, "[2, 8]" },
        };

        [TestCaseSource(nameof(UnionIntervalStringsSource))]
        public void ToString_ShouldReturnCorrectRepresentation(List<EndPointPair<int>> endPointPairs, string expected)
        {
            var interval = new Interval<int>(endPointPairs);

            Assert.That(interval.ToString(), Is.EqualTo(expected));
        }
    }
}
