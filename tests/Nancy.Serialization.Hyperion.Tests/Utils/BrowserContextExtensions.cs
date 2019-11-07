using System.IO;

using Nancy.Testing;

namespace Nancy.Serialization.Hyperion.Tests.Utils
{
    public static class BrowserContextExtensions
    {
        public static void HyperionBody<TModel>(this BrowserContext browserContext, TModel model, ISerializer serializer = null)
        {
            if (serializer == null)
            {
                serializer = new HyperionSerializer();
            }

            IBrowserContextValues browserContextValues = browserContext;
            browserContextValues.Body = new MemoryStream();
            serializer.Serialize("application/x-hyperion", model, browserContextValues.Body);
            browserContext.Header("Content-Type", "application/x-hyperion");
        }
    }
}