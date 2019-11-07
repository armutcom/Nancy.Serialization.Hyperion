using System;
using System.IO;

namespace Nancy.Serialization.Hyperion
{
    public class HyperionResponse<TModel> : Response
    {
        public HyperionResponse(TModel model, ISerializer serializer)
        {
            if (serializer == null)
            {
                throw new InvalidOperationException("Hyperion Serializer not set");
            }

            Contents = model == null ? NoBody : GetHyperionContents(model, serializer);
            ContentType = DefaultContentType;
            StatusCode = HttpStatusCode.OK;
        }

        private static string DefaultContentType => HyperionHelper.HyperionContentType;

        private static Action<Stream> GetHyperionContents(TModel model, ISerializer serializer)
        {
            return stream => serializer.Serialize(DefaultContentType, model, stream);
        }
    }

    public class HyperionResponse : HyperionResponse<object>
    {
        public HyperionResponse(object model, ISerializer serializer) : base(model, serializer)
        {
        }
    }
}