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
        private readonly NanoSoftDbContext _context;

        protected Repository(NanoSoftDbContext context)
        {
            _context = context;
        }

        public virtual async Task AddAsync(TDomain domain) => await _context.AddAsync(domain);

        public virtual async Task AddRangeAsync(IEnumerable<TDomain> domains) => await _context.AddRangeAsync(domains);

        public virtual async Task<TDomain> FindAsync(object id, bool includeRelated = false) => await _context.FindAsync<TDomain>(id);

        public virtual async Task<List<TDomain>> GetAllAsync() => await _context.Set<TDomain>().ToListAsync();

        public virtual async Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<TDomain, TResult>> target)
            => await _context.Set<TDomain>().Select(target).ToListAsync();

        public virtual async Task RemoveAsync(TDomain domain)
        {
            await Task.CompletedTask;
            _context.Remove(domain);
        }

        public virtual async Task RemoveRangeAsync(IEnumerable<TDomain> domains)
        {
            await Task.CompletedTask;
            _context.RemoveRange(domains);
        }
    }
}