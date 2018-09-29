using NanoSoft.Repository;

namespace NanoSoft.Identity
{
    public interface IIdentityUnitOfWork<out TIdentityRepository, TIdentityUser> : IDefaultUnitOfWork
        where TIdentityRepository : IIdentityRepository<TIdentityUser>
        where TIdentityUser : class
    {
        TIdentityRepository IdentityUsers { get; }
    }
}
