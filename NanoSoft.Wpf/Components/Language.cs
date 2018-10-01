using NanoSoft.Wpf.Attributes;
using NanoSoft.Wpf.Resources;

namespace NanoSoft.Wpf.Components
{
    public enum Language
    {
        [SharedPhrase(nameof(SharedPhrases.Arabic))]
        Arabic,

        [SharedPhrase(nameof(SharedPhrases.English))]
        English
    }
}
