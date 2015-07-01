namespace Gu.Wpf.SharedResources.Tests
{
    using System.ComponentModel;

    using NUnit.Framework;

    public class ResourceUriTests
    {
        [TestCase(@"/Gu.Wpf.SharedResources.Tests;component/AllResources.xaml", @"/Gu.Wpf.SharedResources.Tests;component/AllResources.xaml")]
        [TestCase(@"pack://application:,,,/Gu.Wpf.SharedResources.Tests;component/AllResources.xaml", @"/Gu.Wpf.SharedResources.Tests;component/AllResources.xaml")]
        [TestCase("AllResources.xaml", @"/Gu.Wpf.SharedResources.Tests;component/AllResources.xaml")]
        public void Normalizes(string uri, string expected)
        {
            DesignerProperties.SetIsInDesignMode(SharedResourceDictionary.Dummy, true);
            var actual = new ResourceUri(uri);
            Assert.IsFalse(actual.Uri.IsAbsoluteUri);
            Assert.AreEqual(expected, actual.ToString());
            Assert.AreEqual(expected, actual.Uri.ToString());
        }
    }
}
