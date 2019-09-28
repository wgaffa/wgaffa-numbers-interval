using NUnit.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class IntervalSerializationTests
    {
        [Test]
        public void Serialize_ShouldNotThrow()
        {
            var interval = new Interval<float>(5, 10);

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                Assert.That(() => formatter.Serialize(stream, interval), Throws.Nothing);
            }
        }

        [Test]
        public void Deserialize_ShouldReturnEqualObject()
        {
            var interval = new Interval<float>(-5, 4);

            Interval<float> deserialized;
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, interval);

                stream.Position = 0;
                deserialized = (Interval<float>)formatter.Deserialize(stream);
            }

            Assert.That(deserialized, Is.Not.SameAs(interval));
            Assert.That(deserialized, Is.EqualTo(interval));
        }
    }
}
