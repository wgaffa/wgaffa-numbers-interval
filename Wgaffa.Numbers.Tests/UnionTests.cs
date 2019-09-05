using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

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
            var pairs = new Interval<int>[] {
                new Interval<int>(lowerFirst, upperFirst),
                new Interval<int>(lowerSecond, upperSecond)
            };

            var interval = new UnionInterval<int>(pairs);

            Assert.That(interval.Contains(value), Is.EqualTo(expected));
        }

        static readonly List<object[]> UnionIntervalStringsSource = new List<object[]> {
            new object[] { new List<Interval<int>>() { new Interval<int>(2, 5), new Interval<int>(2, 5) }, "[2, 5]" },
            new object[] { new List<Interval<int>>() { new Interval<int>(2, 5), new Interval<int>(7, 8) }, "[2, 5], [7, 8]" },
            new object[] { new List<Interval<int>>() { new Interval<int>(2, 5), new Interval<int>(4, 8) }, "[2, 8]" },
            new object[] {
                new List<Interval<int>>() {
                    new Interval<int>(-4, 13),
                    new Interval<int>(-27, 4),
                    new Interval<int>(-1, 26),
                    new Interval<int>(10, 12),
                    new Interval<int>(1, 22),
                    new Interval<int>(-14, 14),
                    new Interval<int>(-12, -5),
                    new Interval<int>(-26, -18),
                },
                "[-27, 26]"
            },
            new object[] { new List<Interval<int>>() { new Interval<int>(EndPoint<int>.Infinity, 5), new Interval<int>(4, 8) }, "(-Inf, 8]" },
            new object[] { new List<Interval<int>>() { new Interval<int>(2, 5), new Interval<int>(4, new EndPoint<int>(8, false)) }, "[2, 8)" },
        };

        [TestCaseSource(nameof(UnionIntervalStringsSource))]
        public void ToString_ShouldReturnCorrectRepresentation(List<Interval<int>> intervals, string expected)
        {
            var interval = new UnionInterval<int>(intervals);

            Assert.That(interval.ToString(), Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(UnionIntervalStringsSource))]
        public void Union_ShouldCreateCorrectResult(List<Interval<int>> intervals, string expected)
        {
            var first = intervals.First();
            var result = first.Union(intervals.Skip(1));

            Assert.That(result.ToString(), Is.EqualTo(expected));
        }

        static readonly List<object[]> IntervalSetsSource = new List<object[]> {
            new object[]
            {
                new List<Interval<int>>()
                {
                    new Interval<int>(-879, 943), new Interval<int>(-293, -692),
                    new Interval<int>(-809, -159)
                }, -965, new object[] { false, 1 }
            },
            new object[]
            {
                new List<Interval<int>>()
                {
                    new Interval<int>(366, 212), new Interval<int>(-734, -448),
                    new Interval<int>(-587, 433)
                }, 920, new object[] { false, 1 }
            },
            new object[]
            {
                new List<Interval<int>>()
                {
                    new Interval<int>(653, -507), new Interval<int>(-718, 134),
                    new Interval<int>(-995, -169)
                }, -543, new object[] { true, 1 }
            },
            new object[]
            {
                new List<Interval<int>>()
                {
                    new Interval<int>(-170, 323), new Interval<int>(720, -390),
                    new Interval<int>(-429, -843)
                }, -539, new object[] { false, 1 }
            },
            new object[]
            {
                new List<Interval<int>>()
                {
                    new Interval<int>(-897, -977), new Interval<int>(784, -17),
                    new Interval<int>(466, -860)
                }, 67, new object[] { false, 0 }
            }
        };

        [TestCaseSource(nameof(IntervalSetsSource))]
        public void Union_ShouldReturnCorrectInterval(List<Interval<int>> intervals, int value, object[] expectedList)
        {
            var interval = intervals.First().Union(intervals.Skip(1));

            var expected = (bool)expectedList[0];

            Assert.That(interval.Contains(value), Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(IntervalSetsSource))]
        public void Bounds_ShouldReturnCorrectCount(List<Interval<int>> intervals, int value, object[] expectedList)
        {
            var interval = (UnionInterval<int>)intervals.First().Union(intervals.Skip(1));

            var expected = (int)expectedList[1];

            Assert.That(interval.Intervals.Count, Is.EqualTo(expected));
        }
    }
}
