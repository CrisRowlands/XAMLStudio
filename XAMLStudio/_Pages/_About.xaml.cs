using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Windows;

namespace XAMLStudio._Pages
{
    public partial class _About : PhoneApplicationPage
    {
        public _About()
        {
            InitializeComponent();
            txt_version.Text = "Version " + new System.Reflection.AssemblyName(System.Reflection.Assembly.GetExecutingAssembly().FullName).Version.ToString();
        }

        private void AppsClick(object sender, RoutedEventArgs e)
        {
            new MarketplaceSearchTask
            {
                ContentType = MarketplaceContentType.Applications,
                SearchTerms = "CrisRowlands"
            }.Show();
        }
        private void SiteClick(object sender, RoutedEventArgs e)
        {
            new WebBrowserTask
            {
                Uri = new Uri("http://www.crisrowlands.com", UriKind.Absolute)
            }.Show();
        }
        private void TwitClick(object sender, RoutedEventArgs e)
        {
            new WebBrowserTask
            {
                Uri = new Uri("http://www.twitter.com/crisrowlands", UriKind.Absolute)
            }.Show();
        }
        private void EmailClick(object sender, RoutedEventArgs e)
        {
            new EmailComposeTask
            {
                Subject = "XAMLStudio Feedback",
                To = "CrisRowlandsDesign@Live.co.uk"
            }.Show();
        }
    }
}