using NUnit.Framework;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class IntIntervalTests
    {
        [Test]
        public void Merge_ShouldMerge()
        {
            var intervalOne = new IntInterval(5, 7);
            var intervalTwo = new IntInterval(8, 10);

            var result = intervalOne.Merge(intervalTwo);

            var expected = new IntInterval(5, 10);

            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Merge_ShouldThrow_GivenInvalidRanges()
        {
            var intervalOne = new IntInterval(5, EndPoint<int>.Open(7));
            var intervalTwo = new IntInterval(8, 10);

            Assert.That(() => intervalOne.Merge(intervalTwo), Throws.InvalidOperationException);
        }
    }
}
