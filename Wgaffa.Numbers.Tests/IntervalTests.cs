using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
