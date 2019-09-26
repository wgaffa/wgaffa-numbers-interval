using NUnit.Framework;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class DegenerateTests
    {
        [Test]
        public void Degenerate_ShouldReturnTrue_GivenBothOpenWithSameValue()
        {
            var interval = new Interval<float>(0.005f, 0.005f);

            Assert.That(interval.IsDegenerate);
        }

        [Test]
        public void Degenerate_ShouldReturnFalse_GivenDifferentValues()
        {
            var interval = new Interval<float>(0.05f, 1f);

            Assert.That(interval.IsDegenerate, Is.False);
        }

        [Test]
        public void Degenerate_ShouldReturnFalse_GivenEmptyInterval()
        {
            var interval = new Interval<float>(1f, 0.5f);

            Assert.That(interval.IsEmpty);
            Assert.That(interval.IsDegenerate, Is.False);
        }

        [TestCase(true, false)]
        [TestCase(false, false)]
        [TestCase(false, true)]
        public void Degenerate_ShouldReturnFalse_GivenDifferentBoundsWithSameValues(bool left_open, bool right_open)
        {
            var interval = new Interval<float>(new EndPoint<float>(1f, left_open), new EndPoint<float>(1f, right_open));

            Assert.That(interval.IsEmpty);
            Assert.That(interval.IsDegenerate, Is.False);
        }
    }
}
