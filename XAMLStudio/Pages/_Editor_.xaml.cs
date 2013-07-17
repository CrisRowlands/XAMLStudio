using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using XSClasses;

namespace XAMLStudio.Pages
{
    public partial class _Editor_ : PhoneApplicationPage
    {
        public static string _PageName;
        public static string _PageXAML;

        public _Editor_()
        {
            InitializeComponent();
            Loaded += _Editor_Loaded;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("p"))
            {
                _PageName = NavigationContext.QueryString["p"].ToString();
                _PageXAML = FileManager.GetXAML(_PageName);
            }
        }
        private void _Editor_Loaded(object sender, RoutedEventArgs e)
        {
            _BuildPage_();
        }

        private void _BuildPage_()
        {
            try
            {
                UIElement _PageRoot_ = XamlReader.Load(_PageXAML) as UIElement;

                this.Base_Grid_.Children.Clear();
                this.Base_Grid_.Children.Add(_PageRoot_);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to parse/display the xaml.\n\nError message: " + ex.Message, "Error", MessageBoxButton.OK);
            }
        }
    }
}