namespace Gu.Wpf.SharedResources
{
    using System.Collections.Generic;

    /// <summary>
    /// http://blogs.microsoft.co.il/arik/2010/05/28/wpf-single-instance-application/
    /// </summary>
    public interface ISingleInstanceApp
    {
        bool SignalExternalCommandLineArgs(IList<string> args);
    }
}