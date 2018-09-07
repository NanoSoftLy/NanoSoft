using JetBrains.Annotations;
using NanoSoft.Resources;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class LengthAttribute : NanoSoftValidationAttribute
    {
        private int Length { get; }
        private int Min { get; }

        public LengthAttribute(Length length)
        {
            Length = (int)length;
        }

        public LengthAttribute(int min, Length length)
        {
            Length = (int)length;
            Min = min;
        }
        public LengthAttribute(int min, int max)
        {
            Length = max;
            Min = min;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return ValidationResult.Success;

            var s = value.ToString().Trim();

            return s.Length > Length || s.Length < Min
                ? new ValidationResult(string.Format(errorMessage, Length, "{0}"))
                : ValidationResult.Success;
        }

        protected override string DefaultErrorMessage => Min > 0
            ? string.Format(SharedMessages.MinMaxLength, Min, Length, "{0}")
            : string.Format(SharedMessages.MaxLength, Length, "{0}");
    }
}