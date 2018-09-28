using NanoSoft.Identity;
using NanoSoft.Repository;

namespace NanoSoft.EntityFramework.Identity
{
    public interface IIdentityUnitOfWork<out TIdentityRepository, TIdentityUser> : IDefaultUnitOfWork
        where TIdentityRepository : IRepository<TIdentityUser>
        where TIdentityUser : BaseIdentityUser
    {
        TIdentityRepository IdentityUsers { get; }
    }
}
