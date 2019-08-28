using NUnit.Framework;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class IntervalTests
    {
        [Test]
        public void Ctor_ShouldCreateInterval_GivenIntegers()
        {
            var interval = new Interval<int>(3, 24);

            Assert.That(interval.LowerBound.Value, Is.EqualTo(3));
            Assert.That(interval.UpperBound.Value, Is.EqualTo(24));
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
    }
}
