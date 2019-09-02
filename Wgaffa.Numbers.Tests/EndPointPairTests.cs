using NUnit.Framework;
using System.Collections.Generic;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class EndPointPairTests
    {
        static readonly List<object[]> OverlapSource = new List<object[]> {
            new object[] { new EndPointPair<int>(2, 5), new EndPointPair<int>(3, 4), true},
            new object[] { new EndPointPair<int>(2, 5), new EndPointPair<int>(7, 8), false},
            new object[] { new EndPointPair<int>(2, 7), new EndPointPair<int>(6, 8), true},
            new object[] { new EndPointPair<int>(2, 7), new EndPointPair<int>(7, 8), true},
            new object[] { new EndPointPair<int>(2, new EndPoint<int>(7, false)), new EndPointPair<int>(new EndPoint<int>(7, false), 8), false},
            new object[] { new EndPointPair<int>(2, 7), new EndPointPair<int>(new EndPoint<int>(7, false), 8), true},
            new object[] { new EndPointPair<int>(new EndPoint<int>(5, false), 7), new EndPointPair<int>(1, 5), true},
            new object[] { new EndPointPair<int>(5, new EndPoint<int>(7, false)), new EndPointPair<int>(7, 9), true},
            new object[] { new EndPointPair<int>(2, 7), new EndPointPair<int>(1, 5), true},
            new object[] { new EndPointPair<int>(EndPoint<int>.Infinity, 7), new EndPointPair<int>(-75324, 42), true},
            new object[] { new EndPointPair<int>(5, 7), new EndPointPair<int>(2, EndPoint<int>.Infinity), true},
        };

        [TestCaseSource(nameof(OverlapSource))]
        public void Overlap_ShouldReturnCorrectResult(EndPointPair<int> first, EndPointPair<int> second, bool expected)
        {
            var result = first.Overlaps(second);

            Assert.That(result, Is.EqualTo(expected));
        }

        static readonly List<object[]> MergeSource = new List<object[]> {
            new object[] { new EndPointPair<int>(2, 5), new EndPointPair<int>(4, 8), new EndPointPair<int>(2, 8) },
            new object[] { new EndPointPair<int>(2, 5), new EndPointPair<int>(-4, 3), new EndPointPair<int>(-4, 5) },
            new object[] { new EndPointPair<int>(3, 4), new EndPointPair<int>(2, 5), new EndPointPair<int>(2, 5) },
            new object[] { new EndPointPair<int>(3, 4), new EndPointPair<int>(4, 5), new EndPointPair<int>(3, 5) },
            new object[] { new EndPointPair<int>(new EndPoint<int>(3, false), 4), new EndPointPair<int>(4, 5), new EndPointPair<int>(new EndPoint<int>(3, false), 5) },
            new object[] { new EndPointPair<int>(3, 4), new EndPointPair<int>(new EndPoint<int>(4, false), 5), new EndPointPair<int>(3, 5) },
            new object[] { new EndPointPair<int>(3, new EndPoint<int>(4, false)), new EndPointPair<int>(4, 5), new EndPointPair<int>(3, 5) },
            new object[] { new EndPointPair<int>(-34, 4), new EndPointPair<int>(EndPoint<int>.Infinity, 5), new EndPointPair<int>(EndPoint<int>.Infinity, 5) },
            new object[] { new EndPointPair<int>(-34, EndPoint<int>.Infinity), new EndPointPair<int>(EndPoint<int>.Infinity, 5), new EndPointPair<int>(EndPoint<int>.Infinity, EndPoint<int>.Infinity) },
        };

        [TestCaseSource(nameof(MergeSource))]
        public void Merge_ShouldReturnNewPair(EndPointPair<int> first, EndPointPair<int> second, EndPointPair<int> expected)
        {
            var result = first.Merge(second);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Merge_ShouldThrowInvalidOperation_GivenNonOverlappingPairs()
        {
            var first = new EndPointPair<int>(3, new EndPoint<int>(5, false));
            var second = new EndPointPair<int>(new EndPoint<int>(5, false), 8);

            Assert.That(() => first.Merge(second), Throws.InvalidOperationException);
        }
    }
}
