using NanoSoft.Extensions;

namespace NanoSoft.Wpf.Enum
{
    public class EnumContainer
    {
        public object Enum { get; set; }
        public int Index { get; set; }
        public override string ToString()
        {
            return Enum.DisplayName() ?? Enum.ToString();
        }
    }
}