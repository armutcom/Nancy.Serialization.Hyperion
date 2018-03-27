using System.Linq;

namespace Nancy.Serialization.Hyperion
{
    public static class FormatterExtensions
    {
        private static ISerializer _hyperionSerializer;
#if !NETSTANDARD
        public static Response AsHyperion<TModel>(this IResponseFormatter formatter, TModel model,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var serializer = _hyperionSerializer ?? (_hyperionSerializer =
                                 formatter.Serializers.FirstOrDefault(s => s.CanSerialize("application/x-hyperion")));
            var hyperionResponse = new HyperionResonse<TModel>(model, serializer) {StatusCode = statusCode};
            return hyperionResponse;
        }
#endif
    }
}