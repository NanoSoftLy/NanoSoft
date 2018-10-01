using JetBrains.Annotations;
using NanoSoft.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NanoSoft.Extensions
{
    [PublicAPI]
    public static class SharedExtensions
    {
        public static int ToInt32(this decimal value, IntPart part = IntPart.First)
        {
            var number = value.ToString("0.000");

            switch (part)
            {
                case IntPart.First:
                    return int.Parse(number.Split('.')[0]);
                case IntPart.Second:
                    return int.Parse(number.Split('.')[1]);
                default:
                    throw new ArgumentOutOfRangeException(nameof(part), part, null);
            }
        }

        public static string Ellipse(this string value, int max = 50)
            => value.Length <= max ? value : value.Substring(0, max) + "...";

        public static decimal ToRoundedDecimal(this decimal value)
        {
            var first = value.ToInt32();

            var second = value.ToInt32(IntPart.Second);

            if (second == 0 || second == 250 || second == 500 || second == 750)
                return decimal.Parse(first + "." + second);

            if (second > 0 && second < 250)
                return decimal.Parse(first + ".250");
            if (second > 250 && second < 500)
                return decimal.Parse(first + ".500");
            if (second > 500 && second < 750)
                return decimal.Parse(first + ".750");

            return decimal.Parse((first + 1).ToString());
        }


        public static T ToEnum<T>(this string value) where T : struct
            => Enum.TryParse<T>(value, out var enumValue) ? enumValue : default(T);


        public static string ToMd5([NotNull] this string input)
        {
            Check.NotNull(input, nameof(input));

            var md5 = MD5.Create();

            var inputBytes = Encoding.ASCII.GetBytes(input);

            var hash = md5.ComputeHash(inputBytes);


            // step 2, convert byte array to hex string
            var sb = new StringBuilder();

            foreach (var t in hash)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString().ToLower();
        }


        public static string ToBCryptHash(this string input, string salt)
            => BCrypt.Net.BCrypt.HashPassword(input, salt);

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

            var d = type.GetRuntimeField(source.ToString());
            var s = d?.GetCustomAttribute<ResourceBasedAttribute>();
            var r = s?.Display;

            return r;
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

        public static List<T> AsList<T>(this IEnumerable<T> enumerable) => enumerable as List<T> ??
                                                                             enumerable.ToList();
        public static IList<T> AsIList<T>(this IEnumerable<T> enumerable) => enumerable as IList<T> ??
                                                                             enumerable.ToList();
        public static async Task<IList<T>> AsIListAsync<T>(this Task<IEnumerable<T>> enumerable)
        {
            var list = await enumerable;
            return list as IList<T> ?? list.ToList();
        }

        public static string ToExpressionTarget<TObject, TProperty>(
            this Expression<Func<TObject, TProperty>> expression)
            => TryConvert.ToExpressionString(expression);


        public static string ToYearlyNumber(this long number, DateTime date)
            => date.Year + "-" + number;
        public static string ToYearlyNumber(this long number, [NotNull] string date)
            => date.ToDateTime().Year + "-" + number;

        public static long ToPureNumber([NotNull] this string number)
        {
            var insideNumber = number.Contains("-") ? number.Split('-')[1] : number;

            long.TryParse(insideNumber.Trim(), out var newNumber);
            return newNumber;
        }

        public static string ToFormattedString(this string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (value.Contains("{") && value.Contains("}"))
                return string.Format(value, parameterName);

            return value;
        }


        public static T CopyFrom<T>(this T obj1, T obj2)
        {
            foreach (var property in typeof(T).GetRuntimeProperties())
            {
                obj1.SetValue(property.Name, property.GetValue(obj2));
            }

            return obj1;
        }

        public static T CopyTo<T>(this T obj1, T obj2)
        {
            foreach (var property in typeof(T).GetRuntimeProperties())
            {
                obj2.SetValue(property.Name, property.GetValue(obj1));
            }

            return obj2;
        }
    }
}