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
            var count = await query.CountAsync();

            var skippedPages = current - 1;

            var lastPage = CalculateLastPage(count, size);

            var result = await query.Skip(skippedPages * size)
                .Take(size)
                .ToListAsync();

            return new Paginated<TSource>()
            {
                CurrentPage = current,
                LastPage = lastPage,
                PageSize = size,
                Data = result
            };
        }


        public static async Task<Paginated<TResult>> PaginateAsync<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Expression<Func<TSource, TResult>> target)
        {
            var count = await query.CountAsync();

            var skippedPages = current - 1;

            var lastPage = CalculateLastPage(count, size);

            var result = await query.Skip(skippedPages * size)
                .Take(size)
                .Select(target)
                .ToListAsync();

            return new Paginated<TResult>()
            {
                CurrentPage = current,
                LastPage = lastPage,
                PageSize = size,
                Data = result
            };
        }


        public static Paginated<TSource> Paginate<TSource>(this IQueryable<TSource> query, int size, int current)
        {
            var count = query.Count();

            var skippedPages = current - 1;

            var lastPage = CalculateLastPage(count, size);

            var result = query.Skip(skippedPages * size)
                .Take(size)
                .ToList();

            return new Paginated<TSource>()
            {
                CurrentPage = current,
                LastPage = lastPage,
                PageSize = size,
                Data = result
            };
        }


        public static Paginated<TResult> Paginate<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Expression<Func<TSource, TResult>> target)
        {
            var count = query.Count();

            var skippedPages = current - 1;

            var lastPage = CalculateLastPage(count, size);

            var result = query.Skip(skippedPages * size)
                .Take(size)
                .Select(target.Compile())
                .ToList();

            return new Paginated<TResult>()
            {
                CurrentPage = current,
                LastPage = lastPage,
                PageSize = size,
                Data = result
            };
        }


        private static int CalculateLastPage(int totalRows, int pageSize)
        {
            if (pageSize == 0)
                return 0;

            var result = totalRows / pageSize;

            return (totalRows % pageSize) > 0 ? result + 1 : result;
        }
    }
}
