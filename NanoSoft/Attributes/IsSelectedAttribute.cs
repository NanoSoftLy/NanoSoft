using JetBrains.Annotations;
using NanoSoft.Resources;
using System;
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

            if (value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
                return new ValidationResult(errorMessage);

            if (value is Guid guid)
                return guid == default
                    ? new ValidationResult(errorMessage)
                    : ValidationResult.Success;

            var i = (int)value;

            return i <= 0 ? new ValidationResult(errorMessage) : ValidationResult.Success;
        }

        protected override string DefaultErrorMessage => SharedMessages.ShouldSelected;
    }
}