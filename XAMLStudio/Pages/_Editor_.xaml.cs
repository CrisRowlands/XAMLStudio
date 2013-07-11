using Microsoft.Phone.Controls;
using System.Windows.Navigation;

namespace XAMLStudio.Pages
{
    public partial class _Editor_ : PhoneApplicationPage
    {
        string _PageName;

        public _Editor_()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("p"))
            {
                _PageName = NavigationContext.QueryString["p"].ToString();
            }
        }

        //private void _BuildPage_()
        //{
        //    try
        //    {
        //        _PageRoot_ = XamlReader.Load(XMLPAGE.XAML) as UIElement;
        //
        //        this.Base_Grid_.Children.Clear();
        //        this.Base_Grid_.Children.Add(_PageRoot_);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Failed to parse/display the xaml.\n\nError message: " + ex.Message, "Error", MessageBoxButton.OK);
        //    }
        //}
    }
}