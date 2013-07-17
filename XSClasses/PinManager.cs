using Microsoft.Phone.Shell;
using System;
using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace XSClasses
{
    public static class PinManager
    {
        private static Uri BackgroundImageUri = new Uri("/Images/Secondary/M.png", UriKind.Relative);
        private static Uri SmallBackgroundImageUri = new Uri("/Images/Secondary/S.png", UriKind.Relative);

        public static void PinPage(string _Name)
        {
            Uri TileUri = new Uri("/Pages/_Editor_.xaml?p=" + _Name, UriKind.Relative);
            Uri BackBackgroundImageUri = new Uri("isostore:\\Shared\\ShellContent\\" + _Name + ".jpg", UriKind.Absolute);

            if (GetTile("/Pages/_Editor_.xaml?p=" + _Name) == null)
            {
                RadFlipTileData TileData = new RadFlipTileData()
                {
                    Title = _Name,
                    BackgroundImage = BackgroundImageUri,
                    SmallBackgroundImage = SmallBackgroundImageUri,
                    BackBackgroundImage = BackBackgroundImageUri
                };

                LiveTileHelper.CreateTile(TileData, TileUri, false);
            }
            else
            {
                MessageBox.Show("It's already pinned.\nNo need to pin it again, right?", "I can't do that.", MessageBoxButton.OK);
            }
        }
        public static void UnPinPage(string _Name)
        {
            Uri TileUri = new Uri("/Pages/_Editor_.xaml?p=" + _Name, UriKind.Relative);

            if (GetTile("/Pages/_Editor_.xaml?p=" + _Name) != null)
            {
                GetTile("/Pages/_Editor_.xaml?p=" + _Name).Delete();
            }
        }
        private static ShellTile GetTile(string _TileURI)
        {
            return ShellTile.ActiveTiles.FirstOrDefault(tile => tile.NavigationUri.ToString().Contains(_TileURI));
        }
    }
}