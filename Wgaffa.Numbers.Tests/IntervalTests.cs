using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class IntervalTests
    {
        [Test]
        public void Ctor_ShouldCreateInterval_GivenIntegers()
        {
            var interval = new Interval<int>(3, 24);

            var result = interval.Bounds.Single();

            Assert.That(result.Lower.Value, Is.EqualTo(3));
            Assert.That(result.Upper.Value, Is.EqualTo(24));
        }

        [TestCase(1, 10, 5, ExpectedResult = true)]
        [TestCase(100, 250, 57, ExpectedResult = false)]
        [TestCase(30, 35, 75, ExpectedResult = false)]
        [TestCase(15, 15, 15, ExpectedResult = true)]
        [TestCase(-15, 15, 0, ExpectedResult = true)]
        public bool Contains_ShouldReturnCorrectResult(int lower, int upper, int value)
        {
            var interval = new Interval<int>(lower, upper);

            return interval.Contains(value);
        }

        [TestCase(1, 5, ExpectedResult = true)]
        [TestCase(100, 67, ExpectedResult = false)]
        [TestCase(-500, -300, ExpectedResult = true)]
        public bool Contains_ShouldReturnCorrectResult_GivenInfiniteUpperBound(int lower, int value)
        {
            var interval = new Interval<int>(lower, EndPoint<int>.PositiveInfinity);

            return interval.Contains(value);
        }

        [TestCase(1, 5, ExpectedResult = false)]
        [TestCase(100, 67, ExpectedResult = true)]
        [TestCase(-500, -300, ExpectedResult = false)]
        public bool Contains_ShouldReturnCorrectResult_GivenInfiniteLowerBound(int upper, int value)
        {
            var interval = new Interval<int>(EndPoint<int>.NegativeInfinity, upper);

            return interval.Contains(value);
        }

        [TestCase(5, 1, ExpectedResult = true)]
        [TestCase(1, 1, ExpectedResult = true)]
        [TestCase(-77, 1, ExpectedResult = false)]
        public bool Contains_ShouldReturnCorrectResult_GivenInfinityEndPoint_ContextLowerBound(int upper, int value)
        {
            var interval = new Interval<int>(EndPoint<int>.Infinity, upper);

            return interval.Contains(value);
        }

        [TestCase(5, 1, ExpectedResult = false)]
        [TestCase(1, 1, ExpectedResult = true)]
        [TestCase(-77, 1, ExpectedResult = true)]
        public bool Contains_ShouldReturnCorrectResult_GivenInfinityEndPoint_ContextUpperBound(int lower, int value)
        {
            var interval = new Interval<int>(lower, EndPoint<int>.Infinity);

            return interval.Contains(value);
        }

        [TestCase(1, 5, 1, ExpectedResult = true)]
        [TestCase(1, 5, 5, ExpectedResult = true)]
        public bool Contains_ShouldReturnCorrectResult_GivenInclusiveLowerAndUpperBounds(int lower, int upper, int value)
        {
            var interval = new Interval<int>(lower, upper);

            return interval.Contains(value);
        }

        [TestCase(1, 5, 1, ExpectedResult = false)]
        [TestCase(1, 5, 5, ExpectedResult = false)]
        public bool Contains_ShouldReturnCorrectResult_GivenExclusiveLowerAndUpperBounds(int lower, int upper, int value)
        {
            var interval = new Interval<int>(new EndPoint<int>(lower, false), new EndPoint<int>(upper, false));

            return interval.Contains(value);
        }

        [Test]
        public void Contains_ShouldReturnFalse_GivenPositiveInfinityLowerBound()
        {
            var interval = new Interval<int>(EndPoint<int>.PositiveInfinity, 6);

            Assert.That(interval.Contains(3), Is.False);
        }

        static readonly List<object[]> IntervalSource = new List<object[]> {
            new object[] { new Interval<int>(1, 5), "[1, 5]" },
            new object[] { new Interval<int>(5, new EndPoint<int>(75, false)), "[5, 75)" },
            new object[] { new Interval<int>(new EndPoint<int>(2, false), 7), "(2, 7]" },
            new object[] { new Interval<int>(new EndPoint<int>(5, false), new EndPoint<int>(34, false)), "(5, 34)" },
            new object[] { new Interval<int>(EndPoint<int>.NegativeInfinity, EndPoint<int>.PositiveInfinity), "(-Inf, +Inf)" },
            new object[] { new Interval<int>(EndPoint<int>.Infinity, EndPoint<int>.Infinity), "(-Inf, +Inf)" }
        };

        [TestCaseSource(nameof(IntervalSource))]
        public void ToString_ShouldReturnCorrectString(Interval<int> interval, string expected)
        {
            Assert.That(interval.ToString(), Is.EqualTo(expected));
        }

        [TestCase(76, 28, 47)]
        [TestCase(84, 1, 14)]
        [TestCase(61, 57, 28)]
        public void Contains_ShouldReturnFalse_GivenEmptyIntervals(int lower, int upper, int value)
        {
            var interval = new Interval<int>(lower, upper);

            Assert.That(interval.Contains(value), Is.False);
        }

        [TestCase(32, 28, ExpectedResult = true)]
        [TestCase(75, 27, ExpectedResult = true)]
        [TestCase(51, 45, ExpectedResult = true)]
        [TestCase(22, 100, ExpectedResult = false)]
        [TestCase(12, 29, ExpectedResult = false)]
        public bool Empty_ShouldReturnCorrectResult(int lower, int upper)
        {
            var interval = new Interval<int>(lower, upper);

            return interval.IsEmpty;
        }

        static readonly List<Interval<int>> InfiniteIntervalSource = new List<Interval<int>> {
            new Interval<int>(EndPoint<int>.NegativeInfinity, 6),
            new Interval<int>(1, EndPoint<int>.PositiveInfinity),
            new Interval<int>(EndPoint<int>.NegativeInfinity, EndPoint<int>.PositiveInfinity),
            new Interval<int>(EndPoint<int>.Infinity, 6),
            new Interval<int>(1, EndPoint<int>.Infinity),
            new Interval<int>(EndPoint<int>.Infinity, EndPoint<int>.Infinity)
        };

        [TestCaseSource(nameof(InfiniteIntervalSource))]
        public void Empty_ShouldBeFalse_GivenInfiniteBounds(Interval<int> interval)
        {
            Assert.That(interval.IsEmpty, Is.False);
        }
    }
}
