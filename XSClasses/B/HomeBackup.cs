using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using XSClasses;

namespace XAMLStudio
{
    public partial class Home : PhoneApplicationPage
    {
        #region VARS

        private bool Faded = false;
        private string FileName = string.Empty;
        private HomeState state = HomeState.Browse;

        #endregion

        #region MAIN

        public Home()
        {
            InitializeComponent();
            Loaded += Home_Loaded;
            OrientationChanged += Home_OrientationChanged;
            Layout_Root.Children.Add(rec_cover);
            InitAppbar();
        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            if (!Faded)
            {
                Faded = true;
                AnimateBlackOut.Begin();
            }
            else
            {
                rec_cover.Visibility = System.Windows.Visibility.Collapsed;
            }
            ResizeButtons();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadPages();
        }
        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (state == HomeState.Name)
            {
                e.Cancel = true;
                GoToBrowse();
            }
            if (state == HomeState.Template)
            {
                e.Cancel = true;
                GoToName();
            }
        }
        private void Home_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            ResizeButtons();
        }

        private void LoadPages()
        {
            Lst_Files.ItemsSource = null;
            Lst_Files.ItemsSource = FileMan.Files.Values;
            Lst_Files.SelectedIndex = -1;

            if (Lst_Files.Items.Count() == 0)
            {
                txt_nofiles.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                txt_nofiles.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #region FADE IN

        private Rectangle rec_cover = new Rectangle
        {
            Fill = new SolidColorBrush
            {
                Color = Colors.Black
            }
        };
        private Storyboard AnimateBlackOut
        {
            get
            {
                Storyboard _StoryBoard = new Storyboard();

                DoubleAnimationUsingKeyFrames _Animation = new DoubleAnimationUsingKeyFrames();
                Storyboard.SetTarget(_Animation, rec_cover);
                Storyboard.SetTargetProperty(_Animation, new PropertyPath("(UIElement.Opacity)"));

                _Animation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = new TimeSpan(0), Value = 1 });
                _Animation.KeyFrames.Add(new EasingDoubleKeyFrame { KeyTime = new TimeSpan(0, 0, 0, 0, 600), Value = 0, EasingFunction = new PowerEase { EasingMode = EasingMode.EaseOut } });

                _StoryBoard.Children.Add(_Animation);
                _StoryBoard.Completed += _StoryBoard_Completed;

                return _StoryBoard;
            }
        }
        private void _StoryBoard_Completed(object sender, EventArgs e)
        {
            rec_cover.Visibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        #endregion

        #region APPBAR

        private void InitAppbar()
        {
            if (ApplicationBar.MenuItems.Count != 0) return;

            ApplicationBarIconButton new_button = new ApplicationBarIconButton { Text = "new", IconUri = new Uri("/Assets/Icons/appbar.add.rest.png", UriKind.Relative) };
            ApplicationBarMenuItem resources_button = new ApplicationBarMenuItem { Text = "resources" };
            ApplicationBarMenuItem about_button = new ApplicationBarMenuItem { Text = "about" };
            ApplicationBarMenuItem settings_button = new ApplicationBarMenuItem { Text = "settings" };

            new_button.Click += NewClick;
            settings_button.Click += SettingsClick;
            resources_button.Click += ResourceClick;
            about_button.Click += AboutClick;

            ApplicationBar.Buttons.Add(new_button);
            ApplicationBar.MenuItems.Add(resources_button);
            ApplicationBar.MenuItems.Add(about_button);
            ApplicationBar.MenuItems.Add(settings_button);
        }

        private void NewClick(object sender, EventArgs e)
        {
            GoToName();
        }
        private void ResourceClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Sources.xaml", UriKind.Relative));
        }
        private void AboutClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/About.xaml", UriKind.Relative));
        }
        private void SettingsClick(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/Pages/Settings.xaml", UriKind.Relative));
        }

        #endregion

        #region PAGES

        private void Lst_Files_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Lst_Files.SelectedIndex == -1) return;

            NavigationService.Navigate(new Uri("/Pages/_Editor_.xaml?p=" + (Lst_Files.SelectedItem as XAMLFile).FileName, UriKind.Relative));

            Lst_Files.SelectedIndex = -1;
        }
        private void PinClick(object sender, ContextMenuItemSelectedEventArgs e)
        {
            PinManager.PinLevel((sender as RadContextMenuItem).Tag.ToString());
        }
        private void DeleteClick(object sender, ContextMenuItemSelectedEventArgs e)
        {
            string Name = (sender as RadContextMenuItem).Tag.ToString();

            ShellTile Tile = ShellTile.ActiveTiles.FirstOrDefault(tile => tile.NavigationUri.ToString().Contains("/Editor.xaml?p=" + Name));
            if (Tile != null)
            {
                Tile.Delete();
            }

            FileMan.DeleteFile(Name);
            LoadPages();
        }

        #endregion

        #region FILENAME

        private void TxtName_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string PageName = TxtName.Text.Trim();

                if (PageName != string.Empty)
                {
                    if (FileMan.CheckCharacters(PageName))
                    {
                        if (!FileMan.FileExists(PageName))
                        {
                            FileName = PageName;
                            GoToTemplate();
                        }
                        else
                        {
                            MessageBox.Show("This file name is already in use. You must specify a unique name.", "Information.", MessageBoxButton.OK);
                            TxtName.Focus();
                            TxtName.SelectAll();
                        }
                    }
                    else
                    {
                        MessageBox.Show("You can only use alphanumeric characters (letters & numbers).", "Information.", MessageBoxButton.OK);
                        TxtName.Focus();
                        TxtName.SelectAll();
                    }
                }
                else
                {
                    MessageBox.Show("You must specify a file name to continue.", "Information.", MessageBoxButton.OK);
                    TxtName.Focus();
                }
            }
        }
        private void TxtName_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtName.SelectAll();
        }

        #endregion

        #region TEMPLATE

        private void TemplateClick(object sender, RoutedEventArgs e)
        {
            FileMan.CreateFile((sender as Button).Tag.ToString(), FileName);
            GoToBrowse();
            LoadPages();
        }
        private void ResizeButtons()
        {
            if (Orientation == PageOrientation.PortraitUp)
            {
                BtnBasic.Height
                    = BtnPanorama.Height
                    = BtnPivot.Height
                    = BtnEmpty.Height
                    = 460;

                BtnBasic.Width
                    = BtnPanorama.Width
                    = BtnPivot.Width
                    = BtnEmpty.Width
                    = 240;
            }
            else
            {
                BtnBasic.Height
                    = BtnPanorama.Height
                    = BtnPivot.Height
                    = BtnEmpty.Height
                    = 326;

                BtnBasic.Width
                    = BtnPanorama.Width
                    = BtnPivot.Width
                    = BtnEmpty.Width
                    = 160;
            }
        }

        #endregion

        #region ANIMATION

        private void AnimateFileNameBarIn()
        {
            TxtName.Text = "file name";
            int start = -100;
            int end = 0;

            Telerik.Windows.Controls.RadAnimationManager.Play(TxtName, new Telerik.Windows.Controls.RadMoveAnimation
            {
                StartPoint = new Point()
                {
                    X = 0,
                    Y = start
                },

                EndPoint = new Point()
                {
                    X = 0,
                    Y = end
                },

                SpeedRatio = 2,
                AutoReverse = false,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                Easing = new QuinticEase
                {
                    EasingMode = EasingMode.EaseInOut
                }
            });

            TxtName.Focus();
        }
        private void AnimateFileNameBarOut()
        {
            int start = 0;
            int end = -100;

            Telerik.Windows.Controls.RadAnimationManager.Play(TxtName, new Telerik.Windows.Controls.RadMoveAnimation
            {
                StartPoint = new Point()
                {
                    X = 0,
                    Y = start
                },

                EndPoint = new Point()
                {
                    X = 0,
                    Y = end
                },

                SpeedRatio = 2,
                AutoReverse = false,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                Easing = new QuinticEase
                {
                    EasingMode = EasingMode.EaseInOut
                }
            });
        }

        private void AnimateTemplatesIn()
        {
            this.Focus();
            grd_template.Visibility = System.Windows.Visibility.Visible;
            grd_template.Opacity = 1;

            Telerik.Windows.Controls.RadAnimationManager.Play(grd_template, new Telerik.Windows.Controls.RadMoveAnimation
            {
                StartPoint = new Point()
                {
                    X = 800,
                    Y = 0,
                },

                EndPoint = new Point()
                {
                    X = 0,
                    Y = 0
                },

                SpeedRatio = 2,
                AutoReverse = false,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                Easing = new QuinticEase
                {
                    EasingMode = EasingMode.EaseInOut
                }
            });
        }
        private void AnimateTemplatesOut()
        {
            RadFadeAnimation rfa = new RadFadeAnimation
            {
                StartOpacity = 1,
                EndOpacity = 0,
                SpeedRatio = 2,
                AutoReverse = false,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                Easing = new QuinticEase
                {
                    EasingMode = EasingMode.EaseInOut
                }
            };
            rfa.Ended += rfa_Ended;
            Telerik.Windows.Controls.RadAnimationManager.Play(grd_template, rfa);
        }
        private void rfa_Ended(object sender, EventArgs e)
        {
            grd_template.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void AnimateBackgroundIn()
        {
            ApplicationBar.IsVisible = false;

            FileNameBackground.Opacity = 0;
            FileNameBackground.Visibility = System.Windows.Visibility.Visible;

            RadAnimationManager.Play(FileNameBackground, new Telerik.Windows.Controls.RadFadeAnimation
            {
                EndOpacity = 1,
                StartOpacity = 0,
                SpeedRatio = 2,
                AutoReverse = false,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                Easing = new QuinticEase
                {
                    EasingMode = EasingMode.EaseInOut
                }
            });
        }
        private void AnimateBackgroundOut()
        {
            RadFadeAnimation rfa = new RadFadeAnimation
            {
                StartOpacity = 1,
                EndOpacity = 0,
                SpeedRatio = 2,
                AutoReverse = false,
                Duration = new Duration(TimeSpan.FromSeconds(1)),
                Easing = new QuinticEase
                {
                    EasingMode = EasingMode.EaseInOut
                }
            };
            rfa.Ended += back_anim_ended;
            Telerik.Windows.Controls.RadAnimationManager.Play(FileNameBackground, rfa);
        }
        private void back_anim_ended(object sender, EventArgs e)
        {
            FileNameBackground.Visibility = System.Windows.Visibility.Collapsed;
            ApplicationBar.IsVisible = true;
        }

        #endregion

        #region STATES

        private void GoToBrowse()
        {
            AnimateBackgroundOut();

            if (state == HomeState.Name)
            {
                AnimateFileNameBarOut();
            }
            if (state == HomeState.Template)
            {
                AnimateTemplatesOut();
            }

            state = HomeState.Browse;
        }
        private void GoToName()
        {
            AnimateBackgroundIn();

            if (state == HomeState.Template)
            {
                AnimateTemplatesOut();
            }

            AnimateFileNameBarIn();

            state = HomeState.Name;
        }
        private void GoToTemplate()
        {
            if (state == HomeState.Name)
            {
                AnimateFileNameBarOut();
            }

            AnimateTemplatesIn();

            state = HomeState.Template;
        }

        #endregion
    }

    public enum HomeState
    {
        Browse,
        Name,
        Template
    }
}