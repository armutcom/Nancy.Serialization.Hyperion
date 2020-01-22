using System;

using Hyperion;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Nancy.Bootstrapper;
using Nancy.Configuration;
using Nancy.Responses.Negotiation;
using Nancy.Serialization.Hyperion;
using Nancy.TinyIoc;

namespace Nancy.Demo.AspNet.Application
{
    public class DemoBootstrapper : DefaultNancyBootstrapper
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DemoBootstrapper(IServiceProvider serviceProvider, IWebHostEnvironment webHostEnvironment)
        {
            _serviceProvider = serviceProvider;
            _webHostEnvironment = webHostEnvironment;
        }

        protected override Func<ITypeCatalog, NancyInternalConfiguration> InternalConfiguration
        {
            get
            {
                Type[] processors = {typeof(ViewProcessor), typeof(HyperionProcessor), typeof(JsonProcessor), typeof(XmlProcessor)};

                return NancyInternalConfiguration.WithOverrides(x => x.ResponseProcessors = processors);
            }
        }

        public override void Configure(INancyEnvironment environment)
        {
            if (_webHostEnvironment.IsDevelopment())
            {
                environment.Tracing(true, true);
            }
        }

        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            container.Register(_serviceProvider.GetRequiredService<Serializer>());
        }
    }
}