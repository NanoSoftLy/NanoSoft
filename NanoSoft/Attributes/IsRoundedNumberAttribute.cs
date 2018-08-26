using JetBrains.Annotations;
using NanoSoft.Extensions;
using NanoSoft.Resources;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class IsRoundedNumberAttribute : NanoSoftValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (value == null)
                return ValidationResult.Success;

            var number = value.ToString();

            var parsed = decimal.TryParse(number, out var decimalValue);

            if (!parsed)
                return new ValidationResult(errorMessage);

            var decimalPart = decimalValue.ToInt32(IntPart.Second);

            if (decimalPart != 0 && decimalPart != 250 && decimalPart != 500 && decimalPart != 750)
                return new ValidationResult(errorMessage);

            return ValidationResult.Success;
        }

        protected override string DefaultErrorMessage => SharedMessages.ShouldBeRoundedNumber;
    }
}