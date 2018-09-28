using JetBrains.Annotations;
using System.Collections.Generic;

namespace NanoSoft
{
    [PublicAPI]
    public class NullValueDictionary<TKey, TValue> : Dictionary<TKey, TValue>, INullValueDictionary<TKey, TValue>
    {
        TValue INullValueDictionary<TKey, TValue>.this[TKey key] =>
            TryGetValue(key, out var value) ? value : default(TValue);
    }
}