using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Hyperion;

using Nancy.ModelBinding;
using Nancy.Serialization.Hyperion.Settings;
using Nancy.Serialization.Hyperion.Tests.Utils;

using Xunit;

namespace Nancy.Serialization.Hyperion.Tests
{
    public class HyperionDeserializerFixture
    {
        [Fact]
        public void Should_Deserialize_Given_Object()
        {
            var serializer = new Serializer(new SerializerOptions(preserveObjectReferences: HyperionSerializerSettings.Default.PreserveObjectReferences,
                                                                  versionTolerance: HyperionSerializerSettings.Default.VersionTolerance, 
                                                                  ignoreISerializable: HyperionSerializerSettings.Default.IgnoreISerializable));

            var user = new TestUser {Age = 31, Id = Guid.NewGuid(), Name = "Deniz"};


            var hyperionBodyDeserializer = new HyperionBodyDeserializer(serializer);

            var context = new BindingContext
            {
                DestinationType = typeof(TestUser),
                ValidModelBindingMembers = typeof(TestUser).GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(p => new BindingMemberInfo(p))
            };

            using (var bodyStream = new MemoryStream())
            {
                serializer.Serialize(user, bodyStream);

                bodyStream.Position = 0;

                object deserializedUser = hyperionBodyDeserializer.Deserialize("application/x-hyperion", bodyStream, context);

                var testUser = deserializedUser as TestUser;

                Assert.True(testUser != null);

                Assert.Equal(testUser.Name, user.Name);
                Assert.Equal(testUser.Age, user.Age);
                Assert.Equal(testUser.Id, user.Id);
            }
        }

        [Fact]
        public void Should_Return_True_If_Given_Mime_Is_Correct()
        {
            var serializer = new Serializer(new SerializerOptions(preserveObjectReferences: HyperionSerializerSettings.Default.PreserveObjectReferences,
                                                                  versionTolerance: HyperionSerializerSettings.Default.VersionTolerance, 
                                                                  ignoreISerializable: HyperionSerializerSettings.Default.IgnoreISerializable));

            var hyperionBodyDeserializer = new HyperionBodyDeserializer(serializer);

            bool canDeserialize = hyperionBodyDeserializer.CanDeserialize("application/x-hyperion", new BindingContext());

            Assert.True(canDeserialize);
        }
    }
}