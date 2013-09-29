using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace XSClasses
{
    public partial class XSPage : UserControl
    {
        private PhoneApplicationPage CurrentPage
        {
            get
            {
                try
                {

                    return (Application.Current.RootVisual as PhoneApplicationFrame).Content as PhoneApplicationPage;
                }
                catch
                {
                    return null;
                }
            }
        }

        #region MAIN

        public XSPage()
        {
            InitializeComponent();
            this.Loaded += XSPage_Loaded;
            this.Unloaded += XSPage_UnLoaded;
            this.Tap += XSPage_Tap;
            Resize();
        }
        public void Load(string name, BitmapImage screenshot)
        {
            this.TxtBlock_Name.Text = name;
            this.Img_Screenshot.Source = screenshot;
        }
        private void XSPage_Loaded(object sender, RoutedEventArgs e)
        {
            Resize();

            if (CurrentPage != null)
            {
                CurrentPage.OrientationChanged += CurrentPage_OrientationChanged;
            }

        }
        private void XSPage_UnLoaded(object sender, RoutedEventArgs e)
        {
            if (CurrentPage != null)
            {
                CurrentPage.OrientationChanged -= CurrentPage_OrientationChanged;
            }
        }

        #endregion
        #region ROTATION

        private void CurrentPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            Resize();
        }
        private void Resize()
        {
            if (CurrentPage == null)
            {
                return;
            }

            if (CurrentPage.Orientation == PageOrientation.PortraitUp)
            {
                this.Width = 300;
                this.Height = 560;
            }
            else
            {
                this.Width = 210;
                this.Height = 410;
            }
        }

        #endregion

        private void XSPage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TxtBlock_Name.Text != null || TxtBlock_Name.Text != string.Empty)
            {
                CurrentPage.NavigationService.Navigate(new Uri("/_Pages/_Editor.xaml?p=" + TxtBlock_Name.Text, UriKind.Relative));
            }
        }
    }
}