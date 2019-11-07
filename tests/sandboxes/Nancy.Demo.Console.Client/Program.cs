using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Hyperion;

using Nancy.Demo.Models;

namespace Nancy.Demo.Console.Client
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var serializer = new Serializer(new SerializerOptions(
                                                preserveObjectReferences: true,
                                                versionTolerance: true,
                                                ignoreISerializable: true));

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Deniz",
                Age = 31,
                CreateDate = DateTime.Now
            };

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-hyperion"));

            var uri = new Uri("https://localhost:44303/user");

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(user, stream);

                byte[] content = stream.ToArray();
                var byteArrayContent = new ByteArrayContent(content);
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-hyperion");

                HttpResponseMessage postAsync = await client.PostAsync(uri, byteArrayContent);
            }

            HttpResponseMessage getAsync = await client.GetAsync(uri);

            Stream responseAsync = await getAsync.Content.ReadAsStreamAsync();

            user = serializer.Deserialize<User>(responseAsync);
        }
    }
}
