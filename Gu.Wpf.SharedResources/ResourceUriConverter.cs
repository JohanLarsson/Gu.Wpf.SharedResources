using System;
using System.ComponentModel;
using System.Globalization;

namespace Gu.Wpf.SharedResources
{
    public class ResourceUriConverter : TypeConverter
    {
        private static readonly UriTypeConverter Converter = new UriTypeConverter();

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return Converter.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return Converter.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var uri = (Uri)Converter.ConvertFrom(context, culture, value);
            return new ResourceUri(uri);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var resourceUri = (ResourceUri)value;
            return Converter.ConvertTo(context, culture, resourceUri.Uri, destinationType);
        }

        public override bool IsValid(ITypeDescriptorContext context, object value)
        {
            return Converter.IsValid(context, value);
        }
    }
}