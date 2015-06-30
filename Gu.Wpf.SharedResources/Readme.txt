// Step 1: Add the file SingleInstance.cs to your project.

// Step 2: Add a reference to your project: System.Runtime.Remoting

Step 3: Have your application class implement ISingleInstanceApp (defined in SingleInstance.cs).
	The only method in this interface is: bool SignalExternalCommandLineArgs(IList<string> args)
	This method is called when a second instance of your application tries to run. It has an args parameter which is the same as the command line arguments passed to the second instance.
	
Step 4: Define your own Main function that uses the single instance class.
	Your App class should now be similar to this:

    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "YourAppName";

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

Step 5: Set new main entry point
	Select Project Properties –> Application and set “Startup object” to your App class name instead of “(Not Set)”.

Step 6: Cancel the default WPF main function
	Right-click on App.xaml, Properties, set Build Action to “Page” instead of “Application Definition”.