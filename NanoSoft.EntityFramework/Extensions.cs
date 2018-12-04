using Microsoft.EntityFrameworkCore;
using System;
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

            var skipped = CalculateSkipped(total, size, current, startFrom);

            var result = await query.Skip(skipped)
                .Take(size)
                .ToListAsync();

            return new Paginated<TSource>(result, current, size, total);
        }

        public static Task<Paginated<TSource>> PaginateAsync<TSource>(this IQueryable<TSource> query, IPaginationRequest request, StartFrom startFrom = StartFrom.FirstPage)
            => PaginateAsync(query, request.PageSize, request.CurrentPage, startFrom);


        public static async Task<Paginated<TResult>> PaginateAsync<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = await query.CountAsync();

            var skipped = CalculateSkipped(total, size, current, startFrom);

            var result = await query.Skip(skipped)
                .Take(size)
                .Select(target)
                .ToListAsync();

            return new Paginated<TResult>(result, current, size, total);
        }

        public static Task<Paginated<TResult>> PaginateAsync<TSource, TResult>(this IQueryable<TSource> query, IPaginationRequest request, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
            => PaginateAsync(query, request.PageSize, request.CurrentPage, target, startFrom);



        public static Paginated<TSource> Paginate<TSource>(this IQueryable<TSource> query, int size, int current, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = query.Count();

            var skipped = CalculateSkipped(total, size, current, startFrom);

            var result = query.Skip(skipped)
                .Take(size)
                .ToList();

            return new Paginated<TSource>(result, current, size, total);
        }

        public static Paginated<TSource> Paginate<TSource>(this IQueryable<TSource> query, IPaginationRequest request, StartFrom startFrom = StartFrom.FirstPage)
            => Paginate(query, request.PageSize, request.CurrentPage, startFrom);


        public static Paginated<TResult> Paginate<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = query.Count();

            var skipped = CalculateSkipped(total, size, current, startFrom);

            var result = query.Skip(skipped)
                .Take(size)
                .Select(target)
                .ToList();

            return new Paginated<TResult>(result, current, size, total);
        }

        public static Paginated<TResult> Paginate<TSource, TResult>(this IQueryable<TSource> query, IPaginationRequest request, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
            => Paginate(query, request.PageSize, request.CurrentPage, target, startFrom);


        public static IQueryable<TSource> Limit<TSource>(this IQueryable<TSource> query, int size, int current, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = query.Count();

            var skipped = CalculateSkipped(total, size, current, startFrom);

            return query.Skip(skipped)
                .Take(size);
        }

        public static IQueryable<TSource> Limit<TSource>(this IQueryable<TSource> query, IPaginationRequest request, StartFrom startFrom = StartFrom.FirstPage)
            => Limit(query, request.PageSize, request.CurrentPage, startFrom);


        public static IQueryable<TResult> Limit<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = query.Count();

            var skipped = CalculateSkipped(total, size, current, startFrom);

            return query.Skip(skipped)
                .Take(size)
                .Select(target);
        }

        public static IQueryable<TResult> Limit<TSource, TResult>(this IQueryable<TSource> query, IPaginationRequest request, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
            => Limit(query, request.PageSize, request.CurrentPage, target, startFrom);


        private static int CalculateSkipped(int total, int size, int current, StartFrom startFrom)
        {
            if (startFrom == StartFrom.FirstPage)
                return (current - 1) * size;

            if (size == 0)
                return 0;

            var result = (total / size) - 1;

            var remaining = total % size;

            if (remaining > 0)
                return (result + 1) * size;

            return result * size;
        }
    }
}
