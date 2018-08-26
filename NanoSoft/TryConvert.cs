using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NanoSoft
{
    [PublicAPI]
    public static class TryConvert
    {
        public static bool ToDate(string dateString, out DateTime date) => DateTime.TryParse(dateString, out date);

        public static bool ToDeserializedObject<TObject>(string serializedObject, [CanBeNull] out TObject obj)
        {
            try
            {
                obj = JsonConvert.DeserializeObject<TObject>(serializedObject);
                return obj != null;
            }
            catch
            {
                // ignored
            }
            obj = default(TObject);
            return false;
        }

        public static string ToExpressionString<T, TProperty>(Expression<Func<T, TProperty>> exp)
        {
            if (!TryFindMemberExpression(exp.Body, out var memberExp))
                return string.Empty;

            var memberNames = new Stack<string>();
            do
            {
                memberNames.Push(memberExp.Member.Name);
            }
            while (TryFindMemberExpression(memberExp.Expression, out memberExp));
            var members = memberNames.ToList();
            FilterFirstMember(members, exp);

            return string.Join(".", members);
        }

        private static void FilterFirstMember<T, TProperty>(List<string> members, Expression<Func<T, TProperty>> exp)
        {
            var expressionName = exp.Body.ToString();
            var parameterName = exp.Parameters.FirstOrDefault()?.Name;

            if (expressionName.Split('.')[0] != parameterName && members.Count > 1)
                members.RemoveAt(0);
        }

        // code adjusted to prevent horizontal overflow
        private static bool TryFindMemberExpression(Expression exp, out MemberExpression memberExp)
        {
            memberExp = exp as MemberExpression;
            if (memberExp != null)
            {
                // heyo! that was easy enough
                return true;
            }

            // if the compiler created an automatic conversion,
            // it'll look something like...
            // obj => Convert(obj.Property) [e.g., int -> object]
            // OR:
            // obj => ConvertChecked(obj.Property) [e.g., int -> long]
            // ...which are the cases checked in IsConversion
            if (!IsConversion(exp) || !(exp is UnaryExpression))
                return false;

            memberExp = ((UnaryExpression)exp).Operand as MemberExpression;
            return memberExp != null;
        }

        private static bool IsConversion(Expression exp)
            => exp?.NodeType == ExpressionType.Convert ||
               exp?.NodeType == ExpressionType.ConvertChecked;
    }
}
