using System;

namespace NanoSoft.Identity
{
    public abstract class BaseIdentityUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
