namespace Nancy.Serialization.Hyperion.Settings
{
    public sealed class HyperionSerializerSettings
    {
        public static readonly HyperionSerializerSettings Default = new HyperionSerializerSettings(true, true);

        /// <summary>
        ///     When true, it tells <see cref="HyperionSerializer" /> to keep
        ///     track of references in serialized/deserialized object graph.
        /// </summary>
        public readonly bool PreserveObjectReferences;

        /// <summary>
        ///     When true, it tells <see cref="HyperionSerializer" /> to encode
        ///     a list of currently serialized fields into type manifest.
        /// </summary>
        public readonly bool VersionTolerance;

        /// <summary>
        ///     Creates a new instance of a <see cref="HyperionSerializerSettings" />.
        /// </summary>
        /// <param name="preserveObjectReferences">
        ///     Flag which determines if serializer should keep track of references in
        ///     serialized object graph.
        /// </param>
        /// <param name="versionTolerance">Flag which determines if field data should be serialized as part of type manifest.</param>
        public HyperionSerializerSettings(bool preserveObjectReferences, bool versionTolerance)
        {
            PreserveObjectReferences = preserveObjectReferences;
            VersionTolerance = versionTolerance;
        }
    }
}