using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using XSClasses;

namespace XAMLStudio._Pages
{
    public partial class _ViewXaml : PhoneApplicationPage
    {
        private string _Page_Xaml = string.Empty;

        public _ViewXaml()
        {
            InitializeComponent();
            Loaded += _ViewXaml_Loaded;
        }
        private void _ViewXaml_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            txt_xaml.Text = _Page_Xaml;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (NavigationContext.QueryString.ContainsKey("p"))
            {
                _Page_Xaml = FileManager.GetXAML(NavigationContext.QueryString["p"].ToString());
                NavigationContext.QueryString.Clear();
            }
        }

        private void share_click(object sender, System.EventArgs e)
        {

        }
        private void settings_click(object sender, System.EventArgs e)
        {

        }
    }
}