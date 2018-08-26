using System;

namespace NanoSoft
{
    public abstract class UserInfoBase
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
    }
}
