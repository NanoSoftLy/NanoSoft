using JetBrains.Annotations;
using NanoSoft.Extensions;
using NanoSoft.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class TrueTimeAttribute : NanoSoftValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (value == null)
                return ValidationResult.Success;

            if (value is TimeSpan time && time.TotalHours < 24)
                return ValidationResult.Success;

            if (value is string strTime && strTime.ToNullableTimeSpan() != null && strTime.ToTimeSpan().TotalHours < 24)
                return ValidationResult.Success;

            return new ValidationResult(errorMessage);
        }

        protected override string DefaultErrorMessage => SharedMessages.InvalidTime;
    }
}