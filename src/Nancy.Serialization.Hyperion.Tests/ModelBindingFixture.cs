using System;
using Nancy.ModelBinding;
using Nancy.Serialization.Hyperion.Tests.Utils;
using Nancy.Testing;
using Xunit;

namespace Nancy.Serialization.Hyperion.Tests
{
    public class ModelBindingFixture
    {
        [Fact]
        public void Should_Bind_To_A_Class()
        {
            var module = new ConfigurableNancyModule(c => c.Post("/stuff", (_, m) =>
            {
                var stuff = m.Bind<TestUser>();
                return stuff.Id.ToString();
            }));

            var bootstrapper = new TestBootstrapper(config => config.Module(module));

            var user = new TestUser
            {
                Age = 31,
                Id = Guid.NewGuid(),
                Name = "Deniz"
            };

            var browser = new Browser(bootstrapper);
            var browserResponse = browser.Post("/stuff", context =>
            {
                context.HttpRequest();
                context.HyperionBody(user);
            });

            Assert.Equal(user.Id.ToString(), browserResponse.Body.AsString());
        }
    }
}