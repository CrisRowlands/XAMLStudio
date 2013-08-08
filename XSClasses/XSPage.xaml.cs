using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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

        public XSPage()
        {
            InitializeComponent();
            this.Loaded += XSPage_Loaded;
            this.Unloaded += XSPage_UnLoaded;
            this.Tap += XSPage_Tap;
            Resize();
        }

        private void XSPage_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (TxtBlock_Name.Text != null || TxtBlock_Name.Text != string.Empty)
            {
                CurrentPage.NavigationService.Navigate(new Uri("/_Pages/_Editor.xaml?p=" + TxtBlock_Name.Text, UriKind.Relative));
            }
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
        public void Load(string name, BitmapImage screenshot)
        {
            this.TxtBlock_Name.Text = name;
            this.Img_Screenshot.Source = screenshot;

            AnimateIn.Begin();
        }

        private Storyboard AnimateIn
        {
            get
            {
                DoubleAnimationUsingKeyFrames FadeIn = new DoubleAnimationUsingKeyFrames();

                Storyboard.SetTarget(FadeIn, this);
                Storyboard.SetTargetProperty(FadeIn, new PropertyPath("UIElement.Opacity"));

                FadeIn.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0),
                    Value = 0
                });

                FadeIn.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0, 0, 0, 0, 600),
                    Value = 1,
                    EasingFunction = new PowerEase
                    {
                        EasingMode = EasingMode.EaseIn
                    }
                });

                Storyboard FadeInStoryboard = new Storyboard();
                FadeInStoryboard.Children.Add(FadeIn);

                return FadeInStoryboard;
            }
        }
    }
}