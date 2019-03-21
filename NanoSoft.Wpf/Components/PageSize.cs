using NanoSoft.Wpf.Attributes;
using NanoSoft.Wpf.Resources;

namespace NanoSoft.Wpf.Components
{
    public enum PageSize
    {
        [SharedPhrase(nameof(SharedPhrases.TwentyFive))]
        TwentyFive = 25,

        [SharedPhrase(nameof(SharedPhrases.Fifty))]
        Fifty = 50,

        [SharedPhrase(nameof(SharedPhrases.SeventyFive))]
        SeventyFive = 75,

        [SharedPhrase(nameof(SharedPhrases.Hundred))]
        Hundred = 100
    }
}
