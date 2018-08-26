using System;

namespace NanoSoft
{
    public interface IPermissionBasedUser<in TPermission>
    {
        bool Can(TPermission permission, Type type);

        bool Can(TPermission permission);
    }
}
