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
            HyperionSerializerSettings hyperionSerializerSettings = HyperionSerializerSettings.Default;

            _serializer = new Serializer(new SerializerOptions(preserveObjectReferences: hyperionSerializerSettings.PreserveObjectReferences,
                                                               versionTolerance: hyperionSerializerSettings.VersionTolerance,
                                                               ignoreISerializable: hyperionSerializerSettings.IgnoreISerializable));
        }

        public HyperionBodyDeserializer(Serializer serializer)
        {
            _serializer = serializer;
        }

        public bool CanDeserialize(MediaRange mediaRange, BindingContext context)
        {
            return HyperionHelper.IsHyperion(mediaRange);
        }

        public object Deserialize(MediaRange mediaRange, Stream bodyStream, BindingContext context)
        {
            var res = _serializer.Deserialize<object>(bodyStream);

            return res;
        }
    }
}