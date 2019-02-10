using NanoSoft.Identity;
using System;

namespace NanoSoft.IdentityServer
{
    public class IdentityUser : BaseIdentityUser
    {
        public Guid UserId { get; set; }
    }
}
