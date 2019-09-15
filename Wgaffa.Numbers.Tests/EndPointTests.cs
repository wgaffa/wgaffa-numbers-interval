using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class EndPointTests
    {
        [Test]
        public void Value_ShouldReturnCorrectValue()
        {
            var start = new EndPoint<int>(3);

            Assert.That(start.Value, Is.EqualTo(3));
        }

        [Test]
        public void ImplicitConversion_ShouldSetCorrectValue()
        {
            EndPoint<int> start = 2;

            Assert.That(start.Value, Is.EqualTo(2));
        }

        [TestCase(5, 10, ExpectedResult = -1)]
        [TestCase(25, 7, ExpectedResult = 1)]
        [TestCase(-5, 0, ExpectedResult = -1)]
        [TestCase(-5, -5, ExpectedResult = 0)]
        [TestCase(115, 75, ExpectedResult = 1)]
        public int CompareTo_ShouldReturnCorrectOrder(int value, int compareTo)
        {
            var endPoint = new EndPoint<int>(value);

            return endPoint.CompareTo(compareTo);
        }

        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        [TestCase(0)]
        public void CompareTo_ShouldReturnPositiveOrder_GivenPositiveInfinity(int compare)
        {
            var endPoint = EndPoint<int>.PositiveInfinity;

            Assert.That(endPoint.CompareTo(compare), Is.EqualTo(1));
        }

        [TestCase(int.MaxValue)]
        [TestCase(int.MinValue)]
        [TestCase(0)]
        public void CompareTo_ShouldReturnNegativeOrder_GivenNegativeInfinity(int compare)
        {
            var endPoint = EndPoint<int>.NegativeInfinity;

            Assert.That(endPoint.CompareTo(compare), Is.EqualTo(-1));
        }

        [TestCase('f')]
        [TestCase('a')]
        [TestCase('z')]
        public void Infinity_ShouldReturnPositive_GivenCharEndPoints(char x)
        {
            var endPoint = EndPoint<char>.PositiveInfinity;

            Assert.That(endPoint.CompareTo(x), Is.EqualTo(1));
        }

        [Test]
        public void CompareTo_ShouldReturnPositive_GivenLowerIsPositiveToNegativeInfinity()
        {
            var upper = EndPoint<int>.NegativeInfinity;
            var lower = new EndPoint<int>(6);

            Assert.That(lower.CompareTo(upper), Is.EqualTo(1));
        }


        static readonly List<object[]> EndPointComparisonSource = new List<object[]> {
            new object[] { new EndPoint<int>(6), new EndPoint<int>(52), -1 },
            new object[] { new EndPoint<int>(32), new EndPoint<int>(13), 1 },
            new object[] { new EndPoint<int>(65), new EndPoint<int>(65), 0 },
            new object[] { new EndPoint<int>(65), EndPoint<int>.PositiveInfinity, -1 },
            new object[] { new EndPoint<int>(65), EndPoint<int>.NegativeInfinity, 1 },
            new object[] { new EndPoint<int>(65), EndPoint<int>.Infinity, -1 },
            new object[] { EndPoint<int>.PositiveInfinity, new EndPoint<int>(34), 1 },
            new object[] { EndPoint<int>.NegativeInfinity, new EndPoint<int>(34), -1 },
            new object[] { EndPoint<int>.Infinity, new EndPoint<int>(34), 1 },
            new object[] { EndPoint<int>.PositiveInfinity, EndPoint<int>.NegativeInfinity, 1 },
            new object[] { EndPoint<int>.NegativeInfinity, EndPoint<int>.PositiveInfinity, -1 },
            new object[] { EndPoint<int>.NegativeInfinity, EndPoint<int>.NegativeInfinity, 0 },
            new object[] { EndPoint<int>.PositiveInfinity, EndPoint<int>.PositiveInfinity, 0 },
            new object[] { EndPoint<int>.Infinity, EndPoint<int>.Infinity, 0 }
        };

        [TestCaseSource(nameof(EndPointComparisonSource))]
        public void CompareTo_ShouldReturnCorrectOrder(EndPoint<int> first, EndPoint<int> second, int expected)
        {
            var result = first.CompareTo(second);

            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(EndPointComparisonSource))]
        public void Equals_ShouldReturnCorrectResult(EndPoint<int> first, EndPoint<int> second, int equality)
        {
            var expected = equality == 0;

            Assert.That(first.Equals(second), Is.EqualTo(expected));
        }

        [Test]
        public void Ctor_ShouldThrowNullException_GivenNullValue()
        {
            Assert.That(() => new EndPoint<MockClass>(null), Throws.ArgumentNullException);
        }

        class MockClass : IComparable<MockClass>
        {
            public int CompareTo(MockClass other)
            {
                throw new NotImplementedException();
            }
        }
    }
}
