using JetBrains.Annotations;
using NanoSoft.Resources;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class MoreThanZeroAttribute : NanoSoftValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (value == null)
                return ValidationResult.Success;

            var number = value.ToString();

            var parsed = decimal.TryParse(number, out var decimalValue);

            if (!parsed || decimalValue <= 0)
                return new ValidationResult(errorMessage);

            return ValidationResult.Success;
        }

        protected override string DefaultErrorMessage => SharedMessages.InvalidNumber;
    }
}