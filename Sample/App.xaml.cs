namespace ResourceBox
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Microsoft.Shell;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "ResourceBox";

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();

                application.InitializeComponent();
                application.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // handle command line arguments of second instance
            // …
            return true;
        }
    }
}
