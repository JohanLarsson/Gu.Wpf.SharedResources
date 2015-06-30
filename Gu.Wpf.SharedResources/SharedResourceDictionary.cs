﻿namespace Gu.Wpf.SharedResources
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
    }
}
