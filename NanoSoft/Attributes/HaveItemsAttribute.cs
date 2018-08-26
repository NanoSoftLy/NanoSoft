using JetBrains.Annotations;
using NanoSoft.Resources;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class HaveItemsAttribute : NanoSoftValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (value == null)
                return ValidationResult.Success;

            return !(value is IEnumerable<object> list) || !list.Any()
                ? new ValidationResult(errorMessage)
                : ValidationResult.Success;
        }

        protected override string DefaultErrorMessage => SharedMessages.ShouldHaveItems;
    }
}