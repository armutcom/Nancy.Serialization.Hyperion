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
            var hyperionSerializerSettings = HyperionSerializerSettings.Default;

            _serializer = new Serializer(new SerializerOptions(
                preserveObjectReferences: hyperionSerializerSettings.PreserveObjectReferences,
                versionTolerance: hyperionSerializerSettings.VersionTolerance,
                ignoreISerializable: true));
        }

        public HyperionSerializer(Serializer serializer)
        {
            _serializer = serializer;
        }

#if NETSTANDARD
        public bool CanSerialize(MediaRange mediaRange)
        {
            return HyperionHelper.IsHyperion(mediaRange);
        }

        public void Serialize<TModel>(MediaRange mediaRange, TModel model, Stream outputStream)
        {
            _serializer.Serialize(model, outputStream);
        }
#else
        public bool CanSerialize(string contentType)
        {
            return HyperionHelper.IsHyperion(contentType);
        }

        public void Serialize<TModel>(string contentType, TModel model, Stream outputStream)
        {
            _serializer.Serialize(model, outputStream);
        }
#endif

        public IEnumerable<string> Extensions
        {
            get { yield return "hyperion"; }
        }
    }
}