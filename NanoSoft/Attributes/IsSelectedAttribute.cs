using JetBrains.Annotations;
using NanoSoft.Resources;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class IsSelectedAttribute : NanoSoftValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (value == null)
                return ValidationResult.Success;

            var stringValue = value as string ?? value.ToString();

            if (string.IsNullOrWhiteSpace(stringValue))
                return new ValidationResult(errorMessage);

            var i = (int)value;

            return i <= 0 ? new ValidationResult(errorMessage) : ValidationResult.Success;
        }

        protected override string DefaultErrorMessage => SharedMessages.ShouldSelected;
    }
}