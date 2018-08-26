namespace NanoSoft
{
    public interface IRoleBasedUser<in TRole>
    {
        bool Is(TRole role);
    }
}
