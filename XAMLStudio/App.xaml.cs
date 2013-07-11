using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Telerik.Windows.Controls;

namespace XAMLStudio
{
    public partial class App : Application
    {
        #region VARS

        private bool IsInitialized = false;
        public static RadPhoneApplicationFrame RootFrame
        {
            get;
            private set;
        }

        #endregion

        #region ERRORS

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

        #endregion

        #region INIT

        public App()
        {
            UnhandledException += AppException;
            InitializeComponent();
            InitApp();
        }
        private void InitApp()
        {
            if (IsInitialized)
            {
                return;
            }
            RootFrame = new RadPhoneApplicationFrame();
            RootFrame.Navigated += InitComplete;
            RootFrame.NavigationFailed += NavigationFailed;
            IsInitialized = true;
        }
        private void InitComplete(object sender, NavigationEventArgs e)
        {
            if (RootVisual != RootFrame)
            {
                RootVisual = RootFrame;
            }
            RootFrame.Navigated -= InitComplete;
        }

        #endregion
    }
}