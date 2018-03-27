using System.IO;
using Hyperion;
using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using Nancy.Serialization.Hyperion.Settings;

namespace Nancy.Serialization.Hyperion
{
    public class HyperionBodyDeserializer : IBodyDeserializer
    {
        private readonly Serializer _serializer;

        public HyperionBodyDeserializer()
        {
            var hyperionSerializerSettings = HyperionSerializerSettings.Default;

            _serializer = new Serializer(new SerializerOptions(
                preserveObjectReferences: hyperionSerializerSettings.PreserveObjectReferences,
                versionTolerance: hyperionSerializerSettings.VersionTolerance,
                ignoreISerializable: true));
        }

#if NETSTANDARD
        public bool CanDeserialize(MediaRange mediaRange, BindingContext context)
        {
            return HyperionHelper.IsHyperion(mediaRange);
        }

        public object Deserialize(MediaRange mediaRange, Stream bodyStream, BindingContext context)
        {
           var res = _serializer.Deserialize<object>(bodyStream);
           return res;
        }
#else
        public bool CanDeserialize(string contentType, BindingContext context)
        {
            return HyperionHelper.IsHyperion(contentType);
        }

        public object Deserialize(string contentType, Stream bodyStream, BindingContext context)
        {
            var res = _serializer.Deserialize<object>(bodyStream);
            return res;
        }
#endif
    }
}