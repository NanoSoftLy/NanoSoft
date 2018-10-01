namespace NanoSoft
{
    public interface IPermissionBasedUser<in TPermission, in TType>
    {
        bool Can(TPermission permission, TType type);
        bool Can(TPermission permission);
    }
}
