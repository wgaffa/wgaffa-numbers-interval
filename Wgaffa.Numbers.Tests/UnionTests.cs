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
    }
}
