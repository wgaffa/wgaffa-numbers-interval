using NUnit.Framework;
using System.Collections.Generic;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class IntersectTests
    {
        static readonly List<object[]> IntersectIntervalsSource = new List<object[]> {
            new object[] { new Interval<int>(5, 10), new Interval<int>(2, 7), new Interval<int>(5, 7) },
            new object[] { new Interval<int>(1, 10), new Interval<int>(2, 7), new Interval<int>(2, 7) },
            new object[] { new Interval<int>(5, 5), new Interval<int>(2, 7), new Interval<int>(5, 5) },
            new object[] { new Interval<int>(EndPoint<int>.NegativeInfinity, 5), new Interval<int>(2, 7), new Interval<int>(2, 5) },
            new object[] { new Interval<int>(EndPoint<int>.Infinity, 5), new Interval<int>(2, 7), new Interval<int>(2, 5) },
            new object[] { new Interval<int>(5, EndPoint<int>.PositiveInfinity), new Interval<int>(2, 7), new Interval<int>(5, 7) },
            new object[] { new Interval<int>(5, EndPoint<int>.PositiveInfinity), new Interval<int>(2, EndPoint<int>.Infinity), new Interval<int>(5, EndPoint<int>.PositiveInfinity) },
        };

        [TestCaseSource(nameof(IntersectIntervalsSource))]
        public void Intersect_ShouldReturnCorrectInterval(Interval<int> first, Interval<int> second, Interval<int> expected)
        {
            var interval = first.Intersect(second);

            Assert.That(interval, Is.EqualTo(expected));
        }
    }
}
