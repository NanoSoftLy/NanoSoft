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

        public virtual Task AddAsync(TDomain domain) => _context.Set<TDomain>().AddAsync(domain);

        public virtual Task AddRangeAsync(IEnumerable<TDomain> domains) => _context.Set<TDomain>().AddRangeAsync(domains);

        public virtual Task<TDomain> FindAsync(object id, bool includeRelated = false) => _context.Set<TDomain>().FindAsync(id);

        public virtual Task<List<TDomain>> GetAllAsync() => _context.Set<TDomain>().ToListAsync();

        public virtual Task<List<TResult>> GetAllAsync<TResult>(Expression<Func<TDomain, TResult>> target)
            => _context.Set<TDomain>().Select(target).ToListAsync();

        public virtual Task RemoveAsync(TDomain domain)
        {
            _context.Set<TDomain>().Remove(domain);
            return Task.CompletedTask;
        }

        public virtual Task RemoveRangeAsync(IEnumerable<TDomain> domains)
        {
            _context.Set<TDomain>().RemoveRange(domains);
            return Task.CompletedTask;
        }
    }
}