using Microsoft.EntityFrameworkCore;
using NanoSoft.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NanoSoft.EntityFramework
{
    public static class Extensions
    {
        public static async Task<Paginated<TSource>> PaginateAsync<TSource>(this IQueryable<TSource> query, int size, int current, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = await query.CountAsync();

            List<TSource> result;

            if (total > 0)
            {
                var skipped = PaginationExtensions
                    .CalculateSkipped(total, size, current, startFrom);

                result = await query.Skip(skipped)
                    .Take(size)
                    .ToListAsync();
            }
            else
            {
                result = await query.ToListAsync();
            }

            return new Paginated<TSource>(result, current, size, total);
        }

        public static Task<Paginated<TSource>> PaginateAsync<TSource>(this IQueryable<TSource> query, IPaginationRequest request, StartFrom startFrom = StartFrom.FirstPage)
            => PaginateAsync(query, request.PageSize, request.CurrentPage, startFrom);


        public static async Task<Paginated<TResult>> PaginateAsync<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = await query.CountAsync();

            List<TResult> result;

            if (total > 0)
            {
                var skipped = PaginationExtensions
                    .CalculateSkipped(total, size, current, startFrom);

                result = await query.Skip(skipped)
                    .Take(size)
                    .Select(target)
                    .ToListAsync();
            }
            else
            {
                result = await query.Select(target)
                                .ToListAsync();
            }


            return new Paginated<TResult>(result, current, size, total);
        }

        public static Task<Paginated<TResult>> PaginateAsync<TSource, TResult>(this IQueryable<TSource> query, IPaginationRequest request, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
            => PaginateAsync(query, request.PageSize, request.CurrentPage, target, startFrom);

        public static async Task<Paginated<TResult>> PaginateAsync<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Func<TSource, TResult> target, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = await query.CountAsync();

            List<TResult> result;

            if (total > 0)
            {
                var skipped = PaginationExtensions
                    .CalculateSkipped(total, size, current, startFrom);

                result = await Task.Run(() => query.Skip(skipped)
                    .Take(size)
                    .Select(target)
                    .ToList());
            }
            else
            {
                result = await Task.Run(() => query.Select(target)
                                .ToList());
            }


            return new Paginated<TResult>(result, current, size, total);
        }

        public static Task<Paginated<TResult>> PaginateAsync<TSource, TResult>(this IQueryable<TSource> query, IPaginationRequest request, Func<TSource, TResult> target, StartFrom startFrom = StartFrom.FirstPage)
            => PaginateAsync(query, request.PageSize, request.CurrentPage, target, startFrom);
    }
}
