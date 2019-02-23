using NanoSoft.Attributes;
using NanoSoft.Resources;

namespace NanoSoft.Identity
{
    public class LoginModel
    {
        [IsRequired, SharedTitle(nameof(SharedTitles.UserName))]
        public string Name { get; set; }

        [IsRequired, SharedTitle(nameof(SharedTitles.Password))]
        public string Password { get; set; }
    }
}
