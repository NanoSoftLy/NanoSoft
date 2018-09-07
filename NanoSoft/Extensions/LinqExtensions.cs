using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NanoSoft.Extensions
{
    [PublicAPI]
    public static class LinqExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
                collection.Add(item);
        }

        public static void AddRange<T, TSource>(this ICollection<T> collection, IEnumerable<TSource> enumerable, Func<TSource, T> selector)
            => AddRange(collection, enumerable.Select(selector));

        public static void Replace<T>(this ICollection<T> currentCollection, IEnumerable<T> newCollection)
        {
            currentCollection.Clear();
            foreach (var item in newCollection)
                currentCollection.Add(item);
        }

        public static void Replace<T, TSource>(this ICollection<T> currentCollection, IEnumerable<TSource> newCollection, Func<TSource, T> selector)
            => Replace(currentCollection, newCollection.Select(selector));


        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
                action(item);
        }
    }
}