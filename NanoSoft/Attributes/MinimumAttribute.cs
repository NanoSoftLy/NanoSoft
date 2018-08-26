using JetBrains.Annotations;
using NanoSoft.Resources;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class MinimumAttribute : NanoSoftValidationAttribute
    {
        private readonly double _minimumNumber;

        public MinimumAttribute(double minimumNumber)
        {
            _minimumNumber = minimumNumber;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (value == null)
                return ValidationResult.Success;

            var number = value.ToString();

            var parsed = double.TryParse(number, out var doubleValue);

            if (!parsed || doubleValue < _minimumNumber)
                return new ValidationResult(errorMessage);

            return ValidationResult.Success;
        }

        protected override string DefaultErrorMessage => string.Format(SharedMessages.MinimumNumber, _minimumNumber, "{0}");
    }
}