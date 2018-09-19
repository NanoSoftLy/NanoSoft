using JetBrains.Annotations;
using System;

namespace NanoSoft
{
    [PublicAPI]
    public class KeyValuePair
    {
        public Guid Key { get; set; }
        public string Value { get; set; }
    }
}