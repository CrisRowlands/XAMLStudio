using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using XSClasses;

namespace XAMLStudio._Pages
{
    public partial class _Editor_ : PhoneApplicationPage
    {
        #region VARS

        public static string _Page_Name;
        public static string _Page_XAML;

        #endregion

        #region MAIN

        public _Editor_()
        {
            InitializeComponent();
            Loaded += _Editor_Loaded;
            btn_close_menu.Click += _Close_Menu_Click;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("p"))
            {
                _Page_Name = NavigationContext.QueryString["p"].ToString();
                _Page_XAML = FileManager.GetXAML(_Page_Name);

                _Editor_Grid.Visibility = System.Windows.Visibility.Visible;
                _Menu_Visible = true;

                NavigationContext.QueryString.Clear();
            }

        }
        private void _Editor_Loaded(object sender, RoutedEventArgs e)
        {
            _Build_Page();
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (!_Menu_Visible)
            {
                _Show_Menu();
                e.Cancel = true;
            }
            base.OnBackKeyPress(e);
        }

        #endregion

        private void _Build_Page()
        {
            try
            {
                UIElement _PageRoot_ = XamlReader.Load(_Page_XAML) as UIElement;

                this.Base_Grid_.Children.Clear();
                this.Base_Grid_.Children.Add(_PageRoot_);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to parse/display the xaml.\n\nError message: " + ex.Message, "Error", MessageBoxButton.OK);
            }
        }

        #region SHOW / HIDE MENU

        private bool _Menu_Visible = true;
        private void _Close_Menu_Click(object sender, RoutedEventArgs e)
        {
            _Hide_Menu();
        }
        private void _Show_Menu()
        {
            _Menu_Visible = true;
            _Editor_Grid.Visibility = System.Windows.Visibility.Visible;

            Storyboard _S_Board = new Storyboard();

            #region ROTATION
            DoubleAnimation _DA_Rotation = new DoubleAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300)),
                BeginTime = new TimeSpan(0),
                From = 25,
                To = 0,
            };
            Storyboard.SetTarget(_DA_Rotation, _EditorGridProjection);
            Storyboard.SetTargetProperty(_DA_Rotation, new PropertyPath("RotationX"));
            _S_Board.Children.Add(_DA_Rotation);
            #endregion
            #region OPACITY
            DoubleAnimation _DA_Opacity = new DoubleAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300)),
                BeginTime = new TimeSpan(0),
                From = 0,
                To = 1,
            };
            Storyboard.SetTarget(_DA_Opacity, _Editor_Grid);
            Storyboard.SetTargetProperty(_DA_Opacity, new PropertyPath("UIElement.Opacity"));
            _S_Board.Children.Add(_DA_Opacity);
            #endregion

            _S_Board.Begin();
        }
        private void _Hide_Menu()
        {
            _Menu_Visible = false;
            Storyboard S_Board = new Storyboard();
            S_Board.Completed += _Menu_Out_Complete;

            #region ROTATION
            DoubleAnimation _DA_Rotation = new DoubleAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300)),
                BeginTime = new TimeSpan(0),
                From = 0,
                To = 25,
            };

            Storyboard.SetTarget(_DA_Rotation, _EditorGridProjection);
            Storyboard.SetTargetProperty(_DA_Rotation, new PropertyPath("RotationX"));
            S_Board.Children.Add(_DA_Rotation);
            #endregion
            #region OPACITY
            DoubleAnimation _DA_Opacity = new DoubleAnimation
            {
                Duration = new Duration(new TimeSpan(0, 0, 0, 0, 300)),
                BeginTime = new TimeSpan(0),
                From = 1,
                To = 0,
            };
            Storyboard.SetTarget(_DA_Opacity, _Editor_Grid);
            Storyboard.SetTargetProperty(_DA_Opacity, new PropertyPath("UIElement.Opacity"));
            S_Board.Children.Add(_DA_Opacity);
            #endregion

            S_Board.Begin();
        }
        private void _Menu_Out_Complete(object sender, EventArgs e)
        {
            _Editor_Grid.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion
    }
}