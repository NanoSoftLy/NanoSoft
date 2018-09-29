using NanoSoft.Identity;

namespace NanoSoft.Wpf.Services
{
    public interface IIdentityServices<out TRepository, TIdentityUser>
        where TRepository : IIdentityRepository<TIdentityUser>
        where TIdentityUser : BaseIdentityUser
    {
        IIdentityUnitOfWork<TRepository, TIdentityUser> Initialize();
    }
}