using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NanoSoft.Repository
{
    [PublicAPI]
    public interface IReadOnlyRepository<TDomain> where TDomain : class
    {
        [ItemCanBeNull]
        [MustUseReturnValue]
        Task<TDomain> FindAsync([NotNull] object id, bool includeRelated = false);

        [ItemNotNull]
        [MustUseReturnValue]
        Task<List<TDomain>> GetAllAsync();

        [ItemNotNull]
        [MustUseReturnValue]
        Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<TDomain, TResult>> target);
    }
}