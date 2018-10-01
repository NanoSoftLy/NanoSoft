using NanoSoft.Attributes;
using NanoSoft.Wpf.Resources;

namespace NanoSoft.Wpf.Attributes
{
    public class SharedPhraseAttribute : ResourceBasedAttribute
    {
        public SharedPhraseAttribute(string name) : base(typeof(SharedPhrases))
        {
            Display = SharedPhrases.ResourceManager.GetString(name);
        }

        public override string Display { get; }
    }
}
