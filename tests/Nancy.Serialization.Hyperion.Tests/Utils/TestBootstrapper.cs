using System;
using System.Collections.Generic;

using Hyperion;

using Nancy.Bootstrapper;
using Nancy.Responses.Negotiation;
using Nancy.Serialization.Hyperion.Settings;
using Nancy.Testing;
using Nancy.TinyIoc;

namespace Nancy.Serialization.Hyperion.Tests.Utils
{
    public class TestBootstrapper : ConfigurableBootstrapper
    {
        public Serializer Serializer { get; set; }

        public TestBootstrapper()
        {
        }

        public TestBootstrapper(Action<ConfigurableBootstrapperConfigurator> configuration) : base(configuration)
        {
        }

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