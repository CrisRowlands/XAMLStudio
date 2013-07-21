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
        #region VARS

        public static string _PageName;
        public static string _PageXAML;

        #endregion

        #region MAIN

        public _Editor_()
        {
            InitializeComponent();
            Loaded += _Editor_Loaded;
            btn_close_menu.Click += btn_close_menu_Click;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("p"))
            {
                _PageName = NavigationContext.QueryString["p"].ToString();
                _PageXAML = FileManager.GetXAML(_PageName);
            }
            EditorGrid.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void _Editor_Loaded(object sender, RoutedEventArgs e)
        {
            _BuildPage_();
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (EditorGrid.Visibility == System.Windows.Visibility.Visible)
            {
                _HideMenu();
                e.Cancel = true;
            }
            base.OnBackKeyPress(e);
        }

        #endregion

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

        #region SHOW / HIDE MENU

        private void btn_close_menu_Click(object sender, RoutedEventArgs e)
        {
            _HideMenu();
        }
        private void _ShowMenu()
        {
            EditorGrid.Visibility = System.Windows.Visibility.Visible;
        }
        private void _HideMenu()
        {
            EditorGrid.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion
    }
}