using NanoSoft.Identity;
using NanoSoft.Repository;

namespace NanoSoft.Wpf.Services
{
    public interface IIdentityServices<out TRepository, IIdentityUser>
        where TRepository : IRepository<IIdentityUser>
        where IIdentityUser : BaseIdentityUser
    {
        IIdentityUnitOfWork<TRepository> Initialize();
    }
}