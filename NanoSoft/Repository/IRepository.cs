using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NanoSoft.Repository
{
    [PublicAPI]
    public interface IRepository<TDomain> where TDomain : class
    {
        Task AddAsync([NotNull] TDomain domain);
        Task AddRangeAsync([NotNull] IEnumerable<TDomain> domains);
        
        [ItemCanBeNull]
        [MustUseReturnValue]
        Task<TDomain> FindAsync([NotNull] object id, bool includeRelated = false);

        [ItemNotNull]
        [MustUseReturnValue]
        Task<IList<TDomain>> GetAllAsync();

        [ItemNotNull]
        [MustUseReturnValue]
        Task<IList<TResult>> GetAllAsync<TResult>(Expression<Func<TDomain, TResult>> target);

        Task RemoveAsync([NotNull] TDomain domain);
        Task RemoveRangeAsync([NotNull] IEnumerable<TDomain> domains);
    }
}