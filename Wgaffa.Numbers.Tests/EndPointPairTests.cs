using NUnit.Framework;
using System.Collections.Generic;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class EndPointPairTests
    {
        static readonly List<object[]> OverlapSource = new List<object[]> {
            new object[] { new Interval<int>(2, 5), new Interval<int>(3, 4), true},
            new object[] { new Interval<int>(2, 5), new Interval<int>(7, 8), false},
            new object[] { new Interval<int>(2, 7), new Interval<int>(6, 8), true},
            new object[] { new Interval<int>(2, 7), new Interval<int>(7, 8), true},
            new object[] { new Interval<int>(2, new EndPoint<int>(7, false)), new Interval<int>(new EndPoint<int>(7, false), 8), false},
            new object[] { new Interval<int>(2, 7), new Interval<int>(new EndPoint<int>(7, false), 8), true},
            new object[] { new Interval<int>(new EndPoint<int>(5, false), 7), new Interval<int>(1, 5), true},
            new object[] { new Interval<int>(5, new EndPoint<int>(7, false)), new Interval<int>(7, 9), true},
            new object[] { new Interval<int>(2, 7), new Interval<int>(1, 5), true},
            new object[] { new Interval<int>(EndPoint<int>.Infinity, 7), new Interval<int>(-75324, 42), true},
            new object[] { new Interval<int>(5, 7), new Interval<int>(2, EndPoint<int>.Infinity), true},
        };

        [TestCaseSource(nameof(OverlapSource))]
        public void Overlap_ShouldReturnCorrectResult(object first, object second, bool expected)
        {
            var leftPoint = (Interval<int>)first;
            var rightPoint = (Interval<int>)second;
            var result = leftPoint.Overlaps(rightPoint);

            Assert.That(result, Is.EqualTo(expected));
        }

        static readonly List<object[]> MergeSource = new List<object[]> {
            new object[] { new Interval<int>(2, 5), new Interval<int>(4, 8), new Interval<int>(2, 8) },
            new object[] { new Interval<int>(2, 5), new Interval<int>(-4, 3), new Interval<int>(-4, 5) },
            new object[] { new Interval<int>(3, 4), new Interval<int>(2, 5), new Interval<int>(2, 5) },
            new object[] { new Interval<int>(3, 4), new Interval<int>(4, 5), new Interval<int>(3, 5) },
            new object[] { new Interval<int>(new EndPoint<int>(3, false), 4), new Interval<int>(4, 5), new Interval<int>(new EndPoint<int>(3, false), 5) },
            new object[] { new Interval<int>(3, 4), new Interval<int>(new EndPoint<int>(4, false), 5), new Interval<int>(3, 5) },
            new object[] { new Interval<int>(3, new EndPoint<int>(4, false)), new Interval<int>(4, 5), new Interval<int>(3, 5) },
            new object[] { new Interval<int>(-34, 4), new Interval<int>(EndPoint<int>.Infinity, 5), new Interval<int>(EndPoint<int>.Infinity, 5) },
            new object[] { new Interval<int>(-34, EndPoint<int>.Infinity), new Interval<int>(EndPoint<int>.Infinity, 5), new Interval<int>(EndPoint<int>.Infinity, EndPoint<int>.Infinity) },
        };

        [TestCaseSource(nameof(MergeSource))]
        public void Merge_ShouldReturnNewPair(object first, object second, object expected)
        {
            var leftPoint = (Interval<int>)first;
            var rightPoint = (Interval<int>)second;
            var result = leftPoint.Merge(rightPoint);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Merge_ShouldThrowInvalidOperation_GivenNonOverlappingPairs()
        {
            var first = new Interval<int>(3, new EndPoint<int>(5, false));
            var second = new Interval<int>(new EndPoint<int>(5, false), 8);

            Assert.That(() => first.Merge(second), Throws.InvalidOperationException);
        }

        [TestCase(5, null)]
        [TestCase(null, 5)]
        [TestCase(null, null)]
        public void Ctor_ShouldThrowNullException_GivenNullBounds(int? lower, int? upper)
        {
            Assert.That(() => new Interval<int>(lower, upper), Throws.ArgumentNullException);
        }
    }
}
