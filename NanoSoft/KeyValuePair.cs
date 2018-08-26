using JetBrains.Annotations;

namespace NanoSoft
{
    [PublicAPI]
    public class KeyValuePair
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}