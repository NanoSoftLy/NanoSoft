using JetBrains.Annotations;
using NanoSoft.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public abstract class NanoSoftValidationAttribute : ValidationAttribute
    {
        protected string GetErrorMessage(ValidationContext context)
        {
            if (context != null)
                context.DisplayName = context.ObjectType.GetRuntimeProperty(context.MemberName).GetDisplayName() ??
                                      context.DisplayName;

            return DefaultErrorMessage.ToFormattedString(context?.DisplayName);
        }

        protected abstract string DefaultErrorMessage { get; }
    }
}