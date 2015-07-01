using System;
using System.ComponentModel;

namespace Gu.Wpf.SharedResources
{
    /// <summary>
    /// The reason for this class is that the designer does something strange
    /// </summary>
    [TypeConverter(typeof(ResourceUriConverter))]
    public class ResourceUri
    {
        public ResourceUri(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; private set; }
    }
}
