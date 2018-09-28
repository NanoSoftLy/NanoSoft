using JetBrains.Annotations;
using System.Collections.Generic;

namespace NanoSoft
{
    [PublicAPI]
    public interface INullValueDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        [CanBeNull]
        TValue this[TKey key] { get; }

        void Add(TKey key, TValue value);
    }
}