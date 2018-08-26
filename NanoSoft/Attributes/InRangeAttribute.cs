using JetBrains.Annotations;
using NanoSoft.Resources;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class InRangeAttribute : NanoSoftValidationAttribute
    {
        private readonly int _min;
        private readonly int _max;

        public InRangeAttribute(int min, int max)
        {
            _min = min;
            _max = max;
            DefaultErrorMessage = string.Format(SharedMessages.InRange, "{0}", min, max);
        }

        protected override string DefaultErrorMessage { get; }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (value == null)
                return ValidationResult.Success;

            var intValue = decimal.TryParse(value.ToString(), out var num) ? num : (decimal?)null;

            if (intValue == null || intValue < _min || intValue > _max)
                return new ValidationResult(errorMessage);

            return ValidationResult.Success;
        }
    }
}