using JetBrains.Annotations;
using NanoSoft.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace NanoSoft.Extensions
{
    [PublicAPI]
    public static class ReflectionExtensions
    {
        public static T CopyFrom<T>(this T obj1, T obj2)
        {
            foreach (var property in typeof(T).GetRuntimeProperties())
            {
                if (property.CanWrite)
                    obj1.SetValue(property.Name, property.GetValue(obj2));
            }

            return obj1;
        }

        public static T CopyTo<T>(this T obj1, T obj2)
        {
            foreach (var property in typeof(T).GetRuntimeProperties())
            {
                if (property.CanWrite)
                    obj2.SetValue(property.Name, property.GetValue(obj1));
            }

            return obj2;
        }

        [Obsolete]
        public static string DisplayName<TSource>(this TSource source, Expression<Func<TSource, object>> expression)
        {
            var name = expression?.ToExpressionTarget() ?? source.ToString();

            return source.GetType()
                       .GetRuntimeProperty(name)
                       ?.GetCustomAttribute<DisplayAttribute>()
                       ?.Name
                   ?? source.GetType()
                       .GetRuntimeField(name)
                       ?.GetCustomAttribute<DisplayAttribute>()
                       ?.Name;
        }

        [Obsolete]
        public static int? DisplayOrder<TSource>(this TSource source, Expression<Func<TSource, object>> expression)
        {
            var name = expression?.ToExpressionTarget() ?? source.ToString();

            return source.GetType()
                       .GetRuntimeProperty(name)
                       ?.GetCustomAttribute<DisplayAttribute>()
                       ?.Order
                   ?? source.GetType()
                       .GetRuntimeField(name)
                       ?.GetCustomAttribute<DisplayAttribute>()
                       ?.Order;
        }

        public static string DisplayName<TSource>(this TSource source)
        {
            var type = source.GetType();

            var property = type.GetRuntimeProperty(source.ToString());

            if (property != null)
                return property.GetCustomAttribute<ResourceBasedAttribute>()
                    ?.Display;

            return type.GetRuntimeField(source.ToString())
                ?.GetCustomAttribute<ResourceBasedAttribute>()
                ?.Display;
        }

        public static string GetDisplayName(this PropertyInfo info)
        {
            if (info == null)
                return null;

            var title = info.GetCustomAttribute<ResourceBasedAttribute>()?.Display;

            if (title != null)
                return title;

            return info.GetCustomAttribute<DisplayAttribute>()?.Name ?? info.Name;
        }
    }
}