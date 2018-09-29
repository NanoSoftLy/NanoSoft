using NanoSoft.Repository;
using System.Threading.Tasks;

namespace NanoSoft.Identity
{
    public interface IIdentityRepository<TIdentityUser> : IRepository<TIdentityUser>
        where TIdentityUser : class
    {
        Task<TIdentityUser> GetByNameAndPasswordAsync(string name, string password);
    }
}
