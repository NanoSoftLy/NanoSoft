namespace NanoSoft.Wpf.Services
{
    public interface IIdentityServiceProvider<out TIdentityService>
    {
        TIdentityService Initialize();
    }
}