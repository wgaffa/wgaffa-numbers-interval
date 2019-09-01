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
            new object[] { new EndPointPair<int>(2, 7), new EndPointPair<int>(1, 5), true}
        };

        [TestCaseSource(nameof(OverlapSource))]
        public void Overlap_ShouldReturnCorrectResult(EndPointPair<int> first, EndPointPair<int> second, bool expected)
        {
            var result = first.Overlaps(second);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
