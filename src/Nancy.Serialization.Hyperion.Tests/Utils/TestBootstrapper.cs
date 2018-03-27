using System;
using System.Collections.Generic;
using Nancy.Testing;

namespace Nancy.Serialization.Hyperion.Tests.Utils
{
    public class TestBootstrapper : ConfigurableBootstrapper
    {
        public TestBootstrapper()
        {
        }

        public TestBootstrapper(Action<ConfigurableBootstrapperConfigurator> configuration)
            : base(configuration)
        {
        }

        protected override IEnumerable<Type> BodyDeserializers
        {
            get { yield return typeof(HyperionBodyDeserializer); }
        }
    }
}