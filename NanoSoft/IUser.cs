using System;

namespace NanoSoft
{
    public interface IUser
    {
        Guid Id { get; }
        string Name { get; }
    }
}
