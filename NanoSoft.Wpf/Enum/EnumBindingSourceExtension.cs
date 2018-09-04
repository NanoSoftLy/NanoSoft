using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace NanoSoft.Wpf.Enum
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private readonly List<string> _excludes = new List<string>();
        private Type _enumType;
        public Type EnumType
        {
            get => _enumType;
            set
            {
                if (value != _enumType)
                {
                    if (null != value)
                    {
                        var enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }

                    _enumType = value;
                }
            }
        }

        public EnumBindingSourceExtension() { }

        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType;
        }
        public EnumBindingSourceExtension(Type enumType, string exclude)
        {
            _excludes = exclude?.Split('|').ToList() ?? new List<string>();
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == _enumType)
                throw new InvalidOperationException("The EnumType must be specified.");

            var actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            var enumValues = System.Enum.GetValues(actualEnumType);

            if (actualEnumType == _enumType)
                return Display(enumValues);

            var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }

        private List<EnumContainer> Display(Array array)
        {
            var list = new List<EnumContainer>();
            var i = 0;
            foreach (var val in array)
            {
                if (!_excludes.Contains(val.ToString()))
                    list.Add(new EnumContainer()
                    {
                        Enum = val,
                        Index = i
                    });

                i++;
            }

            return list;
        }
    }
}
