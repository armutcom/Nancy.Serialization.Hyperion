using System;

using Nancy.Demo.Models;
using Nancy.ModelBinding;
using Nancy.Serialization.Hyperion;

namespace Nancy.Demo.AspNet.Application.Modules
{
    public sealed class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/user", args =>
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    Name = "Ozgen",
                    Age = 32,
                    CreateDate = DateTime.Now
                };

                // return Response.AsHyperion<User>(user);

                return user;
            });

            Post("/user", o =>
            {
                var user = this.Bind<User>();

                return "OK";
            });
        }
    }
}
