using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NanoSoft.Repository
{
    [PublicAPI]
    public interface IRepository<TDomain> : IReadOnlyRepository<TDomain>
        where TDomain : class
    {
        Task AddAsync([NotNull] TDomain domain);
        Task AddRangeAsync([NotNull] IEnumerable<TDomain> domains);

        Task RemoveAsync([NotNull] TDomain domain);
        Task RemoveRangeAsync([NotNull] IEnumerable<TDomain> domains);
    }
}