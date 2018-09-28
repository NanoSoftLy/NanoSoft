using NanoSoft.Repository;

namespace NanoSoft.Identity
{
    public interface IIdentityUnitOfWork<out TIdentityRepository> : IDefaultUnitOfWork
    {
        TIdentityRepository IdentityUsers { get; }
    }
}
