# Nancy.Serialization.Hyperion
Implementations of the ISerialization and IBodyDeserializer interfaces, based on [Hyperion](https://github.com/akkadotnet/Hyperion), for [Nancy](http://nancyfx.org)

|       | Stable 
|-------|-------|
| Package | [![Build Status](https://img.shields.io/nuget/v/Nancy.Serialization.Hyperion.svg)](https://www.nuget.org/packages/Nancy.Serialization.Hyperion/)       

## Supported Platforms

- .NET 4.6.1 (Desktop / Server)
- [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

## Continuous integration
|       | Windows | Linux |
|-------|-------|----------|
| Build | [![Build Status](https://appveyor-matrix-badges.herokuapp.com/repos/Blind-Striker/nancy-serialization-hyperion/branch/master/1)](https://ci.appveyor.com/project/Blind-Striker/nancy-serialization-hyperion)      | [![Build Status](https://appveyor-matrix-badges.herokuapp.com/repos/Blind-Striker/nancy-serialization-hyperion/branch/master/2)](https://ci.appveyor.com/project/Blind-Striker/nancy-serialization-hyperion)          |


## Usage

Start of by installing the `Nancy.Serialization.Hyperion` nuget

When Nancy detects that the `HyperionSerializer` and `HyperionBodyDeserializer` types are available in the AppDomain, of your application, it will assume you want to use them for `application/x-hyperion` content types.

```csharp
public class HomeModule : NancyModule
{
    public HomeModule()
    {
        Get["/user"] = x =>
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "Ozgen",
                Age = 31,
                CreateDate = DateTime.Now
            };

            return Response.AsHyperion<User>(user);
        };

        Post["PostUser", "/user"] = (o) =>
        {
            User user = this.Bind<User>();

            return "OK";
        };
    }
}
```

### Client request sample

Start of by installing the `Hyperion` nuget

```csharp
static void Main(string[] args)
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

    using (var client = new HttpClient())
    {
        var uri = new Uri("<url>");

        using (var stream = new MemoryStream())
        {
            serializer.Serialize(user, stream);

            byte[] content = stream.ToArray();
            ByteArrayContent byteArrayContent = new ByteArrayContent(content);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-hyperion");

            Task<HttpResponseMessage> postAsync = client.PostAsync(uri, byteArrayContent);

            postAsync.Wait();
        }

        Task<HttpResponseMessage> getAsync = client.GetAsync(uri);

        getAsync.Wait();

        Task<Stream> responseAsync = getAsync.Result.Content.ReadAsStreamAsync();
        Stream contentStream = responseAsync.Result;

        user = serializer.Deserialize<User>(contentStream);
    }
}
```

## License
Licensed under MIT, see [LICENSE](LICENSE) for the full text.
