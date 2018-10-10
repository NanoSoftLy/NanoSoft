using NanoSoft.Resources;
using System.ComponentModel.DataAnnotations;

namespace NanoSoft.Attributes
{
    public class ValidCultureAttribute : NanoSoftValidationAttribute
    {
        protected override string DefaultErrorMessage => SharedMessages.InvalidCulture;

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var errorMessage = GetErrorMessage(context);

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return ValidationResult.Success;


            var strValue = value.ToString();

            switch (strValue)
            {
                case "en-US":
                    return ValidationResult.Success;

                case "ar-LY":
                    return ValidationResult.Success;

                default:
                    return new ValidationResult(errorMessage);
            }
        }
    }
}
