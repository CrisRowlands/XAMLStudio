using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using XSClasses;

namespace XAMLStudio
{
    public partial class _Home : PhoneApplicationPage
    {
        #region MAIN

        public _Home()
        {
            InitializeComponent();
            this.Loaded += Home_Loaded;
            this.OrientationChanged += Home_OrientationChanged;

            txt_name.KeyDown += txt_name_KeyDown;
            txt_name.LostFocus += txt_name_LostFocus;

            btn_pivot.Click += Btn_Template_Click;
            btn_panorama.Click += Btn_Template_Click;
            btn_empty.Click += Btn_Template_Click;
            btn_basic.Click += Btn_Template_Click;
        }
        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateMargin();

            LoadFiles();
        }
        private void Home_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            UpdateMargin();
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (NameShowing)
            {
                AnimateNameOut.Begin();
                FadeBackgroundOut.Begin();
                e.Cancel = true;
            }
            if (TemplateShowing)
            {
                AnimateNameIn.Begin();
                AnimateTemplateOut.Begin();
                e.Cancel = true;
            }
        }
        private void UpdateMargin()
        {
            if (Orientation == PageOrientation.PortraitUp)
            {
                txt_nofiles.Margin = new Thickness(30, 30, 0, 0);
                stk_files.Margin = new Thickness(30, 0, 0, 72);
            }
            if (Orientation == PageOrientation.LandscapeLeft)
            {
                txt_nofiles.Margin = new Thickness(30, 30, 102, 0);
                stk_files.Margin = new Thickness(30, 0, 72, 0);
            }
            if (Orientation == PageOrientation.LandscapeRight)
            {
                txt_nofiles.Margin = new Thickness(102, 30, 0, 0);
                stk_files.Margin = new Thickness(102, 0, 0, 0);
            }
        }

        #endregion
        #region FILES

        private void LoadFiles(bool IsNew = false)
        {
            stk_files.Children.Clear();

            if (FileManager.Files.Keys.Count == 0)
            {
                txt_nofiles.Opacity = 1;
            }
            else
            {
                if (txt_nofiles.Opacity != 0)
                {
                    FadeTextOut.Begin();
                }

                foreach (XSFile xsf in FileManager.Files.Values)
                {
                    XSPage xspi = new XSPage();
                    xspi.Load(xsf.FileName, xsf.ScreenShot, DeleteRefresh);
                    stk_files.Children.Add(xspi);
                }

                if (IsNew)
                {
                    DispatcherTimer dt = new DispatcherTimer
                    {
                        Interval = new TimeSpan(0, 0, 0, 0, 200)
                    };
                    dt.Tick += dt_Tick;
                    dt.Start();
                }
            }
        }

        private void dt_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            scroll_files.ScrollToHorizontalOffset(scroll_files.ScrollableWidth);
        }
        private Storyboard FadeTextOut
        {
            get
            {
                DoubleAnimationUsingKeyFrames FadeText = new DoubleAnimationUsingKeyFrames();

                Storyboard.SetTarget(FadeText, txt_nofiles);
                Storyboard.SetTargetProperty(FadeText, new PropertyPath("(UIElement.Opacity)"));

                FadeText.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0),
                    Value = 1
                });

                FadeText.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0, 0, 0, 0, 600),
                    Value = 0,
                    EasingFunction = new PowerEase
                    {
                        EasingMode = EasingMode.EaseOut
                    }
                });

                Storyboard FadeOutStoryboard = new Storyboard();
                FadeOutStoryboard.Children.Add(FadeText);

                return FadeOutStoryboard;
            }
        }

        private void DeleteRefresh(object sender, System.Windows.Input.GestureEventArgs e)
        {
            DispatcherTimer dt = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, 300)
            };
            dt.Tick += deleteTick;
            dt.Start();
        }
        private void deleteTick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            LoadFiles();
        }

        #endregion
        #region APPBAR

        private void NewClick(object sender, EventArgs e)
        {
            txt_name.Text = string.Empty;
            ApplicationBar.IsVisible = false;
            FadeBackgroundIn.Begin();
            AnimateNameIn.Begin();
        }
        private void ResourceClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/_Pages/_Sources.xaml", UriKind.Relative));
        }
        private void AboutClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/_Pages/_About.xaml", UriKind.Relative));
        }

        #endregion
        #region FILE NAME

        private string FileName = string.Empty;
        private void txt_name_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (txt_name.Text.Trim() == "")
                {
                    MessageBox.Show("File name can't be empty.\nThat would just be silly.", "Error.", MessageBoxButton.OK);
                    return;
                }
                if (!FileManager.CheckCharacters(txt_name.Text))
                {
                    MessageBox.Show("File names can't include symbols.\nIt messes up file creation.", "Error.", MessageBoxButton.OK);
                    return;
                }
                if (FileManager.FileExists(txt_name.Text))
                {
                    MessageBox.Show("This file name already exists.\nPlease choose another one.", "Error.", MessageBoxButton.OK);
                    return;
                }
                FileName = txt_name.Text;
                AnimateTemplateIn.Begin();
            }
        }
        private void txt_name_LostFocus(object sender, RoutedEventArgs e)
        {
            AnimateNameOut.Begin();
            if (!TemplateShowing)
            {
                FadeBackgroundOut.Begin();
            }
        }

        #region ANIMATION
        private bool NameShowing = false;
        private Storyboard AnimateNameIn
        {
            get
            {
                NameShowing = true;
                txt_name.Focus();

                #region SLIDE TEXT BOX IN

                DoubleAnimationUsingKeyFrames SlideTextBoxIn = new DoubleAnimationUsingKeyFrames();

                Storyboard.SetTarget(SlideTextBoxIn, txt_name);
                Storyboard.SetTargetProperty(SlideTextBoxIn, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateY)"));

                SlideTextBoxIn.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0),
                    Value = -100
                });

                SlideTextBoxIn.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0, 0, 0, 0, 300),
                    Value = 0,
                    EasingFunction = new PowerEase
                    {
                        EasingMode = EasingMode.EaseIn
                    }
                });

                #endregion

                Storyboard AnimateNameInStoryboard = new Storyboard();
                AnimateNameInStoryboard.Children.Add(SlideTextBoxIn);

                return AnimateNameInStoryboard;
            }
        }
        private Storyboard AnimateNameOut
        {
            get
            {
                NameShowing = false;
                this.Focus();

                #region SLIDE TEXT BOX OUT

                DoubleAnimationUsingKeyFrames SlideTextBoxOut = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(SlideTextBoxOut, txt_name);
                Storyboard.SetTargetProperty(SlideTextBoxOut, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateY)"));

                SlideTextBoxOut.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0),
                    Value = 0
                });

                SlideTextBoxOut.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0, 0, 0, 0, 300),
                    Value = -100,
                    EasingFunction = new PowerEase
                    {
                        EasingMode = EasingMode.EaseIn
                    }
                });

                #endregion

                Storyboard AnimateNameOutStoryboard = new Storyboard();
                AnimateNameOutStoryboard.Children.Add(SlideTextBoxOut);

                return AnimateNameOutStoryboard;
            }
        }
        #endregion

        #endregion
        #region TEMPLATE

        private void Btn_Template_Click(object sender, RoutedEventArgs e)
        {
            FileManager.CreateFile((sender as Button).Tag.ToString(), FileName);
            FadeBackgroundOut.Begin();
            AnimateNameOut.Begin();
            AnimateTemplateOut.Begin();
            LoadFiles(true);
        }

        #region ANIMATION
        private bool TemplateShowing = false;
        private Storyboard AnimateTemplateIn
        {
            get
            {
                this.Focus();
                TemplateShowing = true;
                scroll_template.ScrollToHorizontalOffset(0);

                #region SLIDE TEMPLATE BOX IN

                DoubleAnimationUsingKeyFrames SlideTemplateBoxIn = new DoubleAnimationUsingKeyFrames();

                Storyboard.SetTarget(SlideTemplateBoxIn, grid_template);
                Storyboard.SetTargetProperty(SlideTemplateBoxIn, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateY)"));

                SlideTemplateBoxIn.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0),
                    Value = -410
                });

                SlideTemplateBoxIn.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0, 0, 0, 0, 300),
                    Value = 0,
                    EasingFunction = new PowerEase
                    {
                        EasingMode = EasingMode.EaseIn
                    }
                });

                #endregion

                #region CREATE STORYBOARD

                Storyboard AnimateTemplateBoxIn = new Storyboard();
                AnimateTemplateBoxIn.Children.Add(SlideTemplateBoxIn);

                #endregion

                return AnimateTemplateBoxIn;
            }
        }
        private Storyboard AnimateTemplateOut
        {
            get
            {
                TemplateShowing = false;

                #region SLIDE TEXT BOX OUT

                DoubleAnimationUsingKeyFrames SlideTemplateBoxOut = new DoubleAnimationUsingKeyFrames();

                Storyboard.SetTarget(SlideTemplateBoxOut, grid_template);
                Storyboard.SetTargetProperty(SlideTemplateBoxOut, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateY)"));

                SlideTemplateBoxOut.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0),
                    Value = 0
                });

                SlideTemplateBoxOut.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0, 0, 0, 0, 300),
                    Value = -410,
                    EasingFunction = new PowerEase
                    {
                        EasingMode = EasingMode.EaseIn
                    }
                });

                #endregion

                #region CREATE STORYBOARD

                Storyboard AnimateTemplateOutStoryboard = new Storyboard();
                AnimateTemplateOutStoryboard.Children.Add(SlideTemplateBoxOut);

                #endregion

                return AnimateTemplateOutStoryboard;
            }
        }
        #endregion

        #endregion
        #region BACKGROUND ANIMATION

        private Storyboard FadeBackgroundIn
        {
            get
            {
                rect_back.Opacity = 0;
                rect_back.Visibility = System.Windows.Visibility.Visible;

                DoubleAnimationUsingKeyFrames FadeBackIn = new DoubleAnimationUsingKeyFrames();

                Storyboard.SetTarget(FadeBackIn, rect_back);
                Storyboard.SetTargetProperty(FadeBackIn, new PropertyPath("(UIElement.Opacity)"));

                FadeBackIn.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0),
                    Value = 0
                });

                FadeBackIn.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0, 0, 0, 0, 300),
                    Value = 1,
                    EasingFunction = new PowerEase
                    {
                        EasingMode = EasingMode.EaseOut
                    }
                });

                Storyboard FadeInStoryboard = new Storyboard();
                FadeInStoryboard.Children.Add(FadeBackIn);

                return FadeInStoryboard;
            }
        }
        private Storyboard FadeBackgroundOut
        {
            get
            {
                rect_back.Opacity = 1;
                rect_back.Visibility = System.Windows.Visibility.Visible;

                DoubleAnimationUsingKeyFrames FadeBackOut = new DoubleAnimationUsingKeyFrames();

                Storyboard.SetTarget(FadeBackOut, rect_back);
                Storyboard.SetTargetProperty(FadeBackOut, new PropertyPath("(UIElement.Opacity)"));

                FadeBackOut.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0),
                    Value = 1
                });

                FadeBackOut.KeyFrames.Add(new EasingDoubleKeyFrame
                {
                    KeyTime = new TimeSpan(0, 0, 0, 0, 300),
                    Value = 0,
                    EasingFunction = new PowerEase
                    {
                        EasingMode = EasingMode.EaseOut
                    }
                });

                Storyboard FadeOutStoryboard = new Storyboard();
                FadeOutStoryboard.Children.Add(FadeBackOut);
                FadeOutStoryboard.Completed += FadeOutStoryboard_Completed;

                return FadeOutStoryboard;
            }
        }
        private void FadeOutStoryboard_Completed(object sender, EventArgs e)
        {
            rect_back.Visibility = System.Windows.Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
        }

        #endregion
    }
}