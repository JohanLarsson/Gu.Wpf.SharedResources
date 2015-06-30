using System.Text;
using System.Windows.Controls;

namespace Gu.Wpf.SharedResources
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;

    /// <summary>
    /// http://www.wpftutorial.net/MergedDictionaryPerformance.html
    /// http://ithoughthecamewithyou.com/post/Merging-Resource-Dictionaries-for-fun-and-profit.aspx
    /// The shared resource dictionary is a specialized resource dictionary
    /// that loads it content only once. If a second instance with the same source
    /// is created, it only merges the resources from the cache.
    /// </summary>
    public class SharedResourceDictionary : ResourceDictionary
    {
        public static readonly DependencyProperty SharedResourcesProperty = DependencyProperty.RegisterAttached(
            "SharedResources",
            typeof(ResourceUri),
            typeof(SharedResourceDictionary),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.NotDataBindable,
                OnSharedResourcesChanged));

        /// <summary>
        /// Internal cache of loaded dictionaries 
        /// </summary>
        private static readonly Dictionary<Uri, ResourceDictionary> SharedDictionaries = new Dictionary<Uri, ResourceDictionary>();

        public static bool IsInDesignMode
        {
            get
            {
                return (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject))
                                               .DefaultValue;
            }
        }

        /// <summary>
        /// Local member of the source uri
        /// </summary>
        private Uri _sourceUri;

        /// <summary>
        /// Gets or sets the uniform resource identifier (URI) to load resources from.
        /// </summary>
        public new Uri Source
        {
            get
            {
                return _sourceUri;
            }
            set
            {
                _sourceUri = value;

                if (!SharedDictionaries.ContainsKey(value))
                {
                    // If the dictionary is not yet loaded, load it by setting
                    // the source of the base class
                    base.Source = value;

                    // add it to the cache
                    SharedDictionaries.Add(value, this);
                }
                else
                {
                    // If the dictionary is already loaded, get it from the cache
                    MergedDictionaries.Add(SharedDictionaries[value]);
                }
            }
        }

        public static void SetSharedResources(ContentControl element, ResourceUri value)
        {
            element.SetValue(SharedResourcesProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(ContentControl))]
        public static ResourceUri GetSharedResources(ContentControl element)
        {
            return (ResourceUri)element.GetValue(SharedResourcesProperty);
        }

        private static void OnSharedResourcesChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                //return;
                var message = string.Format("Cannot use null as uri. Expecting: {0}/ResourceBox;component/AllResources.xaml", Environment.NewLine);
                throw new ArgumentNullException("e", message);
            }
            var resourceUri = (ResourceUri)e.NewValue;
            //if (IsInDesignMode)
            //{
            //    var message = string.Format("Uri: {0}", resourceUri.Uri);
            //    throw new ArgumentException(message);
            //}
            ResourceDictionary rd;
            if (!SharedDictionaries.TryGetValue(resourceUri.Uri, out rd))
            {
                try
                {
                    rd = (ResourceDictionary)Application.LoadComponent(resourceUri.Uri);
                }
                catch (Exception ex)
                {
                    var message = string.Format("Failed loading {0}", resourceUri);
                    throw new ArgumentException(message, ex);
                }
                SharedDictionaries.Add(resourceUri.Uri, rd);
            }
            Add(o, rd);
        }

        private static void Add(DependencyObject o, ResourceDictionary rd)
        {
            var fe = o as FrameworkElement;
            if (fe != null)
            {
                fe.Resources.MergedDictionaries.Add(rd);
                return;
            }

            var fce = o as FrameworkContentElement;
            if (fce != null)
            {
                fce.Resources.MergedDictionaries.Add(rd);
                return;
            }
        }
    }
}
