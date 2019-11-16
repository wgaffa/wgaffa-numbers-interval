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

        [Test]
        public void Merge_ShouldReturnIntInterval()
        {
            var intervalOne = new IntInterval(5, 7);
            var intervalTwo = new IntInterval(7, 10);

            var result = intervalOne.Merge(intervalTwo);

            Assert.That(result, Is.TypeOf<IntInterval>());
        }

        [Test]
        public void Merge_ShouldMerge_GivenChaining()
        {
            var intervalOne = new IntInterval(3, 8);
            var intervalTwo = new IntInterval(9, 10);
            var intervalThree = new IntInterval(11, 15);

            var result = intervalOne
                .Merge(intervalTwo)
                .Merge(intervalThree);

            var expected = new IntInterval(3, 15);

            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
