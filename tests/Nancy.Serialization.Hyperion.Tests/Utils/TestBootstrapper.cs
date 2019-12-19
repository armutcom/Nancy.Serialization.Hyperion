using System;
using System.Collections.Generic;

using Hyperion;

using Nancy.Serialization.Hyperion.Settings;
using Nancy.Testing;
using Nancy.TinyIoc;

namespace Nancy.Serialization.Hyperion.Tests.Utils
{
    public class TestBootstrapper : ConfigurableBootstrapper
    {
        public TestBootstrapper()
        {
        }

        public TestBootstrapper(Action<ConfigurableBootstrapperConfigurator> configuration) : base(configuration)
        {
        }

        public Serializer Serializer { get; set; }

        protected override IEnumerable<Type> BodyDeserializers
        {
            get
            {
                yield return typeof(HyperionBodyDeserializer);
            }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            HyperionSerializerSettings hyperionSerializerSettings = HyperionSerializerSettings.Default;

            Serializer = new Serializer(new SerializerOptions(preserveObjectReferences: hyperionSerializerSettings.PreserveObjectReferences,
                                                              versionTolerance: hyperionSerializerSettings.VersionTolerance,
                                                              ignoreISerializable: hyperionSerializerSettings.IgnoreISerializable));

            container.Register(Serializer);
        }
    }
}