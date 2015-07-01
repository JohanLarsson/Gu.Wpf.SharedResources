using System;
using System.ComponentModel;

namespace Gu.Wpf.SharedResources
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows;

    /// <summary>
    /// The reason for this class is that the designer does something strange
    /// </summary>
    [TypeConverter(typeof(ResourceUriConverter))]
    public class ResourceUri
    {
        private static readonly Dictionary<Uri, ResourceUri> Cache = new Dictionary<Uri, ResourceUri>(); 
        public static readonly string Pattern = @"(?<pack>pack://application:,,,)?(/?(?<app>.+)(?<component>;component))?/?(?<path>.+\.xaml)";
        public ResourceUri(Uri uri)
            : this(uri.OriginalString)
        {
        }

        public ResourceUri(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException("uri");
            }
            var normalized = Normalize(uri);
            Uri = new Uri(normalized, UriKind.Relative);
        }

        public Uri Uri { get; private set; }

        public static ResourceUri Create(Uri value)
        {
            ResourceUri uri;
            if (!Cache.TryGetValue(value, out uri))
            {
                uri = new ResourceUri(value);
                Cache.Add(value, uri);
            }
            return uri;
        }

        public static ResourceUri CreateForGeneric(Assembly assembly)
        {
            // "/Controls;component/Themes/Generic.xaml"
            var uriString = string.Format(@"/{0};component/Themes/Generic.xaml", assembly.GetName().Name);
            var uri = new Uri(uriString, UriKind.Relative);
            return Create(uri);
        }

        public static bool operator ==(ResourceUri left, ResourceUri right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ResourceUri left, ResourceUri right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Uri.ToString();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return Equals((ResourceUri)obj);
        }

        public override int GetHashCode()
        {
            return Uri.GetHashCode(); // GetHashCode is case insensitive
        }

        protected bool Equals(ResourceUri other)
        {
            return Uri.Equals(other.Uri);
        }

        /// <summary>
        /// Maybe this is a bad idea. Useful as they are used as keys in the cache.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private static string Normalize(string uri)
        {
            var match = Regex.Match(uri, Pattern);
            if (!match.Success)
            {
                throw new ArgumentException(string.Format("Could not normalize: {0}", uri), "uri");
            }
            if (!match.Groups["app"].Success)
            {
                var entryAssembly = GetEntryAssembly();
                var normalized = string.Format("/{0};component/{1}",
                    entryAssembly.GetName().Name,
                    match.Groups["path"].Value);
                return normalized;
            }
            else
            {
                var normalized = string.Format("/{0};component/{1}",
                                    match.Groups["app"].Value,
                                    match.Groups["path"].Value);
                return normalized;
            }
        }

        /// <summary>
        /// http://stackoverflow.com/a/974737/1069200
        /// </summary>
        /// <returns></returns>
        internal static Assembly GetEntryAssembly()
        {
            if (SharedResourceDictionary.IsInDesignMode)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                var entryAssembly = assemblies.Where(a => a.EntryPoint != null)
                                              .FirstOrDefault(a => a.GetTypes().Any(t => t.IsSubclassOf(typeof(Application))));
                return entryAssembly;
            }
            // Should work at runtime
            return Assembly.GetEntryAssembly();
        }
    }
}
