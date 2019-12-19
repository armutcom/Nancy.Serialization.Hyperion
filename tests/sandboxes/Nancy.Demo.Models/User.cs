using System;

namespace Nancy.Demo.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public DateTime CreateDate { get; set; }
    }
}