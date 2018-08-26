using JetBrains.Annotations;
using NanoSoft.Resources;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class NumericAttribute : NanoSoftValidationAttribute
    {
        public NumericAttribute()
        {
            DefaultErrorMessage = SharedMessages.IsNumeric;
        }

        protected override string DefaultErrorMessage { get; }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return ValidationResult.Success;

            return long.TryParse(value.ToString(), out var _)
                ? ValidationResult.Success
                : new ValidationResult(errorMessage);
        }
    }
}