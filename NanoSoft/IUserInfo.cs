using System;

namespace NanoSoft
{
    public interface IUserInfo
    {
        Guid Id { get; }
        string Name { get; }
    }
}
