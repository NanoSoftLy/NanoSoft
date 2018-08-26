using JetBrains.Annotations;
using NanoSoft.Resources;
using System;

namespace NanoSoft.Attributes
{
    [PublicAPI]
    public class SharedTitleAttribute : ResourceBasedAttribute
    {
        public SharedTitleAttribute(string name) : base(typeof(SharedTitles))
        {
            Display = ResourceType.GetString(name);
        }
        public SharedTitleAttribute(string name, Type type) : base(type)
        {
            Display = ResourceType.GetString(name);
        }

        public override string Display { get; }
    }
}