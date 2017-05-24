using System;
using System.Windows.Markup;

namespace MyAnimeViewer.Utility
{
    /// <summary>
    /// Extension allowing for the databinding of enums
    /// 
    /// source: http://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/
    /// </summary>
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type m_enumType;
        public Type EnumType
        {
            get { return m_enumType; }
            set
            {
                if (value != m_enumType)
                {
                    if (null != value)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }
                }

                m_enumType = value;
            }
        }

        public EnumBindingSourceExtension()
        {
        }

        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == m_enumType)
                throw new InvalidOperationException("The EnumType must be specified.");

            Type actualEnumType = Nullable.GetUnderlyingType(m_enumType) ?? m_enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == m_enumType)
                return enumValues;

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}
