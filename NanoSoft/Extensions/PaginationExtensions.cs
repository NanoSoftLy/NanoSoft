using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NanoSoft.Extensions
{
    public static class PaginationExtensions
    {
        public static Paginated<TSource> Paginate<TSource>(this IEnumerable<TSource> query, int size, int current, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = query.Count();

            List<TSource> result;

            if (total > 0)
            {
                var skipped = CalculateSkipped(total, size, current, startFrom);

                result = query.Skip(skipped)
                    .Take(size)
                    .ToList();
            }
            else
            {
                result = query.ToList();
            }

            return new Paginated<TSource>(result, current, size, total);
        }

        public static Paginated<TSource> Paginate<TSource>(this IEnumerable<TSource> query, IPaginationRequest request, StartFrom startFrom = StartFrom.FirstPage)
            => Paginate(query, request.PageSize, request.CurrentPage, startFrom);


        public static Paginated<TResult> Paginate<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = query.Count();

            List<TResult> result;

            if (total > 0)
            {
                var skipped = CalculateSkipped(total, size, current, startFrom);

                result = query.Skip(skipped)
                    .Take(size)
                    .Select(target)
                    .ToList();
            }
            else
            {
                result = query.Select(target).ToList();
            }

            return new Paginated<TResult>(result, current, size, total);
        }

        public static Paginated<TResult> Paginate<TSource, TResult>(this IEnumerable<TSource> query, int size, int current, Func<TSource, TResult> target, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = query.Count();

            List<TResult> result;

            if (total > 0)
            {
                var skipped = CalculateSkipped(total, size, current, startFrom);

                result = query.Skip(skipped)
                    .Take(size)
                    .Select(target)
                    .ToList();
            }
            else
            {
                result = query.Select(target).ToList();
            }

            return new Paginated<TResult>(result, current, size, total);
        }

        public static Paginated<TResult> Paginate<TSource, TResult>(this IQueryable<TSource> query, IPaginationRequest request, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
            => Paginate(query, request.PageSize, request.CurrentPage, target, startFrom);

        public static Paginated<TResult> Paginate<TSource, TResult>(this IEnumerable<TSource> query, IPaginationRequest request, Func<TSource, TResult> target, StartFrom startFrom = StartFrom.FirstPage)
            => Paginate(query, request.PageSize, request.CurrentPage, target, startFrom);


        public static IEnumerable<TSource> Limit<TSource>(this IEnumerable<TSource> query, int size, int current, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = query.Count();

            var skipped = CalculateSkipped(total, size, current, startFrom);

            return query.Skip(skipped)
                .Take(size);
        }

        public static IEnumerable<TSource> Limit<TSource>(this IEnumerable<TSource> query, IPaginationRequest request, StartFrom startFrom = StartFrom.FirstPage)
            => Limit(query, request.PageSize, request.CurrentPage, startFrom);


        public static IQueryable<TResult> Limit<TSource, TResult>(this IQueryable<TSource> query, int size, int current, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = query.Count();

            var skipped = CalculateSkipped(total, size, current, startFrom);

            return query.Skip(skipped)
                .Take(size)
                .Select(target);
        }

        public static IEnumerable<TResult> Limit<TSource, TResult>(this IEnumerable<TSource> query, int size, int current, Func<TSource, TResult> target, StartFrom startFrom = StartFrom.FirstPage)
        {
            var total = query.Count();

            var skipped = CalculateSkipped(total, size, current, startFrom);

            return query.Skip(skipped)
                .Take(size)
                .Select(target);
        }

        public static IQueryable<TResult> Limit<TSource, TResult>(this IQueryable<TSource> query, IPaginationRequest request, Expression<Func<TSource, TResult>> target, StartFrom startFrom = StartFrom.FirstPage)
            => Limit(query, request.PageSize, request.CurrentPage, target, startFrom);

        public static IEnumerable<TResult> Limit<TSource, TResult>(this IEnumerable<TSource> query, IPaginationRequest request, Func<TSource, TResult> target, StartFrom startFrom = StartFrom.FirstPage)
            => Limit(query, request.PageSize, request.CurrentPage, target, startFrom);


        public static int CalculateSkipped(int total, int size, int current, StartFrom startFrom)
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
