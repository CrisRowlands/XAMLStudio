using Microsoft.Phone.Shell;
using System;
using System.Linq;
using System.Windows;

namespace XSClasses
{
    public static class PinManager
    {
        public static void PinPage(string _FileName)
        {
            if (!PinManager.TileExists(_FileName))
            {
                ShellTile.Create(new Uri("/_Pages/_Editor.xaml?p=" + _FileName, UriKind.Relative), new FlipTileData
                {
                    Title = _FileName,
                    BackBackgroundImage = new Uri("isostore:\\Shared\\ShellContent\\" + _FileName + ".jpg", UriKind.Absolute),
                    BackgroundImage = new Uri("/Images/Secondary/M.png", UriKind.Relative),
                    SmallBackgroundImage = new Uri("/Images/Secondary/S.png", UriKind.Relative)
                }, false);
            }
            else
            {
                MessageBox.Show("It's already pinned.", "I can't do that.", MessageBoxButton.OK);
            }
        }
        public static void UnPinPage(string _FileName)
        {
            Uri TileUri = new Uri("/_Pages/_Editor.xaml?p=" + _FileName, UriKind.Relative);

            if (!TileExists(_FileName))
            {
                ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("p=" + _FileName)).Delete();
            }
        }
        private static bool TileExists(string _FileName)
        {
            ShellTile TileToCheck = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains("p=" + _FileName));

            return (TileToCheck != null);
        }
    }
}