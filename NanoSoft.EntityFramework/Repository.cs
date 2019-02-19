using Microsoft.EntityFrameworkCore;
using NanoSoft.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NanoSoft.EntityFramework
{
    public abstract class Repository<TDomain> : IRepository<TDomain> where TDomain : class
    {
        private readonly IDbContext _context;

        protected Repository(IDbContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(TDomain domain) => await _context.Set<TDomain>().AddAsync(domain);

        public virtual async Task AddRangeAsync(IEnumerable<TDomain> domains) => await _context.Set<TDomain>().AddRangeAsync(domains);

        public virtual async Task<TDomain> FindAsync(object id, bool includeRelated = false) => await _context.Set<TDomain>().FindAsync(id);

        public virtual async Task<List<TDomain>> GetAllAsync() => await _context.Set<TDomain>().ToListAsync();

        public virtual async Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<TDomain, TResult>> target)
            => await _context.Set<TDomain>().Select(target).ToListAsync();

        public virtual async Task RemoveAsync(TDomain domain)
        {
            await Task.CompletedTask;
            _context.Set<TDomain>().Remove(domain);
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<TDomain> domains)
        {
            await Task.CompletedTask;
            _context.Set<TDomain>().RemoveRange(domains);
        }
    }
}