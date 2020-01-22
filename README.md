# Nancy.Serialization.Hyperion

Implementations of the ISerialization and IBodyDeserializer interfaces, based on [Hyperion](https://github.com/akkadotnet/Hyperion), for [Nancy](http://nancyfx.org)

|         | Stable                                                                                                                                           |
| ------- | ------------------------------------------------------------------------------------------------------------------------------------------------ |
| Package | [![Build Status](https://img.shields.io/nuget/v/Nancy.Serialization.Hyperion.svg)](https://www.nuget.org/packages/Nancy.Serialization.Hyperion/) |

## Supported Platforms

- .NET 4.6.1 (Desktop / Server)
- [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard)

## Continuous integration

|       | Windows                                                                                                                                                                                                      | Linux                                                                                                                                                                                                        |
| ----- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Build | [![Build Status](https://appveyor-matrix-badges.herokuapp.com/repos/Blind-Striker/nancy-serialization-hyperion/branch/master/1)](https://ci.appveyor.com/project/Blind-Striker/nancy-serialization-hyperion) | [![Build Status](https://appveyor-matrix-badges.herokuapp.com/repos/Blind-Striker/nancy-serialization-hyperion/branch/master/2)](https://ci.appveyor.com/project/Blind-Striker/nancy-serialization-hyperion) |

## Using with Asp.Net Core and Owin

Start of by installing the `Nancy`, `Nancy.Serialization.Hyperion`, `Microsoft.AspNetCore.Owin` nuget packages

Configure your `Startup.cs` as below

```csharp
public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<Serializer>(provider =>
        {
            HyperionSerializerSettings hyperionSerializerSettings = HyperionSerializerSettings.Default;

            return new Serializer(new SerializerOptions(preserveObjectReferences: hyperionSerializerSettings.PreserveObjectReferences,
                                                        versionTolerance: hyperionSerializerSettings.VersionTolerance,
                                                        ignoreISerializable: hyperionSerializerSettings.IgnoreISerializable));
        });

        // If using Kestrel:
        services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });

        // If using IIS:
        services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseOwin(action => action.UseNancy(options => options.Bootstrapper = new DemoBootstrapper(app.ApplicationServices, env)));
    }
}
```

Then we need to create our `DemoBootstrapper.cs` which responsible for both registering `HyperionProcessor` to Nancy configuration and registering Hyperion serializer to `TinyIoCContainer`.

```csharp
public class DemoBootstrapper : DefaultNancyBootstrapper
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DemoBootstrapper(IServiceProvider serviceProvider, IWebHostEnvironment webHostEnvironment)
    {
        _serviceProvider = serviceProvider;
        _webHostEnvironment = webHostEnvironment;
    }

    protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
    {
        get
        {
            Type[] processors = {typeof(ViewProcessor), typeof(HyperionProcessor), typeof(JsonProcessor), typeof(XmlProcessor)};

            return NancyInternalConfiguration.WithOverrides(x => x.ResponseProcessors = processors);
        }
    }

    public override void Configure(INancyEnvironment environment)
    {
        if (_webHostEnvironment.IsDevelopment())
        {
            environment.Tracing(true, true);
        }
    }

    protected override void ConfigureApplicationContainer(TinyIoCContainer container)
    {
        base.ConfigureApplicationContainer(container);

        container.Register(_serviceProvider.GetRequiredService<Serializer>());
    }
}
```

### NancyModule example

When Nancy detects that the `HyperionSerializer` and `HyperionBodyDeserializer` types, it will assume you want to use them for `application/x-hyperion` content types.

```csharp
public sealed class HomeModule : NancyModule
{
    public HomeModule()
    {
        Get("/user", args =>
        {
            var user = new User() {Id = Guid.NewGuid(), Name = "Ozgen", Age = 32, CreateDate = DateTime.Now};

            // return Response.AsHyperion<User>(user);

            return user;
        });

        Post("/user", o =>
        {
            var user = this.Bind<User>();

            return "OK";
        });
    }
}
```

### Client request example

Start of by installing the `Hyperion` nuget

```csharp
private static async Task Main(string[] args)
{
    var serializer = new Serializer(new SerializerOptions(preserveObjectReferences: true, versionTolerance: true, ignoreISerializable: true));

    var user = new User() {Id = Guid.NewGuid(), Name = "Deniz", Age = 31, CreateDate = DateTime.Now};

    using var client = new HttpClient();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-hyperion"));

    var uri = new Uri("<adress>/user");

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
```

See [Nancy.Demo.AspNet.Application](https://github.com/armutcom/Nancy.Serialization.Hyperion/tree/master/tests/sandboxes/Nancy.Demo.AspNet.Application) and [Nancy.Demo.Console.Client](https://github.com/armutcom/Nancy.Serialization.Hyperion/tree/master/tests/sandboxes/Nancy.Demo.Console.Client) sandbox applications for details.

## Custom Nancy components for Hyperion

- [HyperionBodyDeserializer](https://github.com/armutcom/Nancy.Serialization.Hyperion/blob/master/src/Nancy.Serialization.Hyperion/HyperionBodyDeserializer.cs)
- [HyperionSerializer](https://github.com/armutcom/Nancy.Serialization.Hyperion/blob/master/src/Nancy.Serialization.Hyperion/HyperionSerializer.cs)
- [HyperionResponse](https://github.com/armutcom/Nancy.Serialization.Hyperion/blob/master/src/Nancy.Serialization.Hyperion/HyperionResponse.cs)
- [HyperionProcessor](https://github.com/armutcom/Nancy.Serialization.Hyperion/blob/master/src/Nancy.Serialization.Hyperion/HyperionProcessor.cs)

## License

Licensed under MIT, see [LICENSE](LICENSE) for the full text.
