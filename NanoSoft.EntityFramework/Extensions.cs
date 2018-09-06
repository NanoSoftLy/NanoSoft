using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NanoSoft.EntityFramework
{
    public static class Extensions
    {
        public static async Task<Paginated<TSource>> PaginateAsync<TSource>(this IQueryable<TSource> query, int size, int current)
        {
            var total = await query.CountAsync();

            var skippedPages = current - 1;

            var result = await query.Skip(skippedPages * size)
                .Take(size)
                .ToListAsync();

            return new Paginated<TSource>(result, current, size, total);
        }


        public static async Task<Paginated<TResult>> PaginateAsync<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Expression<Func<TSource, TResult>> target)
        {
            var total = await query.CountAsync();

            var skippedPages = current - 1;

            var result = await query.Skip(skippedPages * size)
                .Take(size)
                .Select(target)
                .ToListAsync();

            return new Paginated<TResult>(result, current, size, total);
        }


        public static Paginated<TSource> Paginate<TSource>(this IQueryable<TSource> query, int size, int current)
        {
            var total = query.Count();

            var skippedPages = current - 1;

            var result = query.Skip(skippedPages * size)
                .Take(size)
                .ToList();

            return new Paginated<TSource>(result, current, size, total);
        }


        public static Paginated<TResult> Paginate<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Expression<Func<TSource, TResult>> target)
        {
            var total = query.Count();

            var skippedPages = current - 1;

            var result = query.Skip(skippedPages * size)
                .Take(size)
                .Select(target.Compile())
                .ToList();

            return new Paginated<TResult>(result, current, size, total);
        }
    }
}
