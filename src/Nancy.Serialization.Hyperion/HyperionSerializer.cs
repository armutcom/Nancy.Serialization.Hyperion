using System.Collections.Generic;
using System.IO;

using Hyperion;

using Nancy.Responses.Negotiation;
using Nancy.Serialization.Hyperion.Settings;

namespace Nancy.Serialization.Hyperion
{
    public class HyperionSerializer : ISerializer
    {
        private readonly Serializer _serializer;

        public HyperionSerializer()
        {
            HyperionSerializerSettings hyperionSerializerSettings = HyperionSerializerSettings.Default;

            _serializer = new Serializer(new SerializerOptions(preserveObjectReferences: hyperionSerializerSettings.PreserveObjectReferences,
                                                               versionTolerance: hyperionSerializerSettings.VersionTolerance,
                                                               ignoreISerializable: hyperionSerializerSettings.IgnoreISerializable));
        }

        public HyperionSerializer(Serializer serializer)
        {
            _serializer = serializer;
        }

        public bool CanSerialize(MediaRange mediaRange)
        {
            return HyperionHelper.IsHyperion(mediaRange);
        }

        public void Serialize<TModel>(MediaRange mediaRange, TModel model, Stream outputStream)
        {
            _serializer.Serialize(model, outputStream);
        }

        public IEnumerable<string> Extensions
        {
            get
            {
                yield return "hyperion";
            }
        }
    }
}