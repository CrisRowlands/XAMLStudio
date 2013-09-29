using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Telerik.Windows.Controls;

namespace XAMLStudio
{
    public partial class App : Application
    {
        public App()
        {
            UnhandledException += AppException;
            InitializeComponent();
            InitApp();
        }
        private void InitApp()
        {
            if (HasAppInit)
            {
                return;
            }
            RootFrame = new RadPhoneApplicationFrame();
            RootFrame.Navigated += CompleteInit;
            RootFrame.NavigationFailed += NavigationFailed;
            HasAppInit = true;
        }
        private void CompleteInit(object sender, NavigationEventArgs e)
        {
            if (RootVisual != RootFrame)
            {
                RootVisual = RootFrame;
            }
            RootFrame.Navigated -= CompleteInit;
        }

        private bool HasAppInit = false;
        public static RadPhoneApplicationFrame RootFrame { get; private set; }
        private RadDiagnostics Diagnostics = new RadDiagnostics
        {
            EmailTo = "CrisRowlandsDesign@Live.com",
            ApplicationName = "TouchMeQuick",
            EmailSubject = "TouchMeQuick Error report."
        };

        private void App_Activated(object sender, Microsoft.Phone.Shell.ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved)
            {
                ApplicationUsageHelper.OnApplicationActivated();
            }
        }
        private void App_Launching(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            ApplicationUsageHelper.Init("2.5.0.0");
        }
        private void NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
        private void AppException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
    }
}