using NUnit.Framework;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Wgaffa.Numbers.Tests
{
    [TestFixture]
    public class EndPointSerializationTests
    {
        [Test]
        public void Serialize_ShouldNotThrow()
        {
            var endPoint = EndPoint<float>.Open(-15.5f);

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                Assert.That(() => formatter.Serialize(stream, endPoint), Throws.Nothing);
            }
        }

        [Test]
        public void Deserialize_ShouldEqual()
        {
            var endPoint = EndPoint<float>.Closed(4.3f);

            var formatter = new BinaryFormatter();
            EndPoint<float> deserialized;
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, endPoint);

                stream.Position = 0;
                deserialized = (EndPoint<float>)formatter.Deserialize(stream);
            }

            Assert.That(deserialized, Is.Not.SameAs(endPoint));
            Assert.That(deserialized, Is.EqualTo(endPoint));
        }
    }
}
