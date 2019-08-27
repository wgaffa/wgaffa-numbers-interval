using NUnit.Framework;

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
    }
}
