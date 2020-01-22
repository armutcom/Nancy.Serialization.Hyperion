using System;
using System.IO;

using Hyperion;

using Nancy.Serialization.Hyperion.Settings;
using Nancy.Serialization.Hyperion.Tests.Utils;

using Xunit;

namespace Nancy.Serialization.Hyperion.Tests
{
    public class HyperionSerializerFixture
    {
        [Fact]
        public void Should_Return_True_If_Given_Mime_Is_Correct()
        {
            var hyperionDeserializer = new HyperionSerializer();

            bool canDeserialize = hyperionDeserializer.CanSerialize("application/x-hyperion");

            Assert.True(canDeserialize);
        }

        [Fact]
        public void Should_Serialize_Given_Object()
        {
            var serializer = new Serializer(new SerializerOptions(preserveObjectReferences: HyperionSerializerSettings.Default.PreserveObjectReferences,
                                                                  versionTolerance: HyperionSerializerSettings.Default.VersionTolerance, ignoreISerializable: true));

            var user = new TestUser {Age = 31, Id = Guid.NewGuid(), Name = "Deniz"};

            var hyperionSerializer = new HyperionSerializer(serializer);

            using (var bodyStream = new MemoryStream())
            {
                hyperionSerializer.Serialize("application/x-hyperion", user, bodyStream);

                bodyStream.Position = 0;

                object deserializedUser = serializer.Deserialize(bodyStream);

                var testUser = deserializedUser as TestUser;

                Assert.True(testUser != null);

                Assert.Equal(testUser.Name, user.Name);
                Assert.Equal(testUser.Age, user.Age);
                Assert.Equal(testUser.Id, user.Id);
            }
        }
    }
}