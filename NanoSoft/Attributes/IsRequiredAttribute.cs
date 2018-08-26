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

            var stringValue = value as string ?? value.ToString();

            return string.IsNullOrWhiteSpace(stringValue) ? new ValidationResult(errorMessage) : ValidationResult.Success;
        }
    }
}