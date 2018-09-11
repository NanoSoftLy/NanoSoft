using JetBrains.Annotations;
using NanoSoft.Resources;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class IsRequiredAttribute : NanoSoftValidationAttribute
    {
        protected override string DefaultErrorMessage => SharedMessages.IsRequired;

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (value == null)
                return new ValidationResult(errorMessage);

            if (value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
                return new ValidationResult(errorMessage);

            return ValidationResult.Success;
        }
    }
}