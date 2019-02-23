using System;

namespace NanoSoft.Identity
{
    public interface IIdentityResult
    {
        Guid Id { get; }
        string Name { get; }
        string Token { get; }
    }
}
