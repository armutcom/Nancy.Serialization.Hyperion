namespace Nancy.Serialization.Hyperion
{
    public static class FormatterExtensions
    {
        /// <summary>
        /// Serializes the <paramref name="model" /> to Hyperion and returns it to the
        /// agent, optionally using the <paramref name="statusCode" />.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <param name="formatter">The formatter.</param>
        /// <param name="model">The model to serialize.</param>
        /// <param name="statusCode">The HTTP status code. Defaults to <see cref="F:Nancy.HttpStatusCode.OK" />.</param>
        public static Response AsHyperion<TModel>(this IResponseFormatter formatter, TModel model, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            ISerializer serializer = formatter.SerializerFactory.GetSerializer(HyperionHelper.HyperionContentType);
            var hyperionResponse = new HyperionResponse<TModel>(model, serializer) {StatusCode = statusCode};

            return hyperionResponse;
        }
    }
}