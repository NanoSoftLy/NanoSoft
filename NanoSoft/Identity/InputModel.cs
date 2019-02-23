using NanoSoft.Attributes;
using NanoSoft.Resources;

namespace NanoSoft.Identity
{
    public class InputModel : IValidatable
    {
        [IsRequired, SharedTitle(nameof(SharedTitles.UserName))]
        public string Name { get; set; }

        [IsRequired, SharedTitle(nameof(SharedTitles.Password))]
        public string Password { get; set; }

        [IsRequired, SharedTitle(nameof(SharedTitles.ConfirmPassword))]
        public string ConfirmPassword { get; set; }


        void IValidatable.Validate(IValidator validator)
        {
            if (Password != ConfirmPassword)
                validator.AddError(m => ConfirmPassword, SharedMessages.PasswordNotMatch);
        }
    }
}
