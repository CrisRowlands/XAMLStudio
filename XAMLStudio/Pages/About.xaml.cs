using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Windows;

namespace XAMLStudio.Pages
{
    public partial class About : PhoneApplicationPage
    {
        public About()
        {
            InitializeComponent();
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