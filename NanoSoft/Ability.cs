using System;

namespace NanoSoft
{
    public abstract class Ability<TPermission>
    {
        public Ability(TPermission permission)
        {
            Permission = permission;
            Type = null;
        }

        public Ability(TPermission permission, Type type)
        {
            Permission = permission;
            Type = type;
        }

        public TPermission Permission { get; protected set; }
        public Type Type { get; protected set; }
    }
}
