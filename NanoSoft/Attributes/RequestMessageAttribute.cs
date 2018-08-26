using NanoSoft.Resources;

namespace NanoSoft.Attributes
{
    public class RequestMessageAttribute : ResourceBasedAttribute
    {
        public RequestMessageAttribute(string name) : base(typeof(SharedMessages))
        {
            Display = ResourceType.GetString(name);
        }

        public override string Display { get; }
    }
}