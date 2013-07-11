using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace XSClasses
{
    public static class FileManager
    {
        public static Dictionary<string, XSFile> Files
        {
            get
            {
                return (Dictionary<string, XSFile>)SettingsManager.GetEntry("Files", new Dictionary<string, XSFile>());
            }
            set
            {
                SettingsManager.SetEntry("Files", value);
            }
        }

        public static XSFile GetFile(string Name)
        {
            return Files[Name];
        }

        public static void CreateFile(string Template, string Name)
        {
            #region XAML

            string XamlString = string.Empty;

            StreamResourceInfo _StreamResourceInfo = System.Windows.Application.GetResourceStream(new Uri("XAML/" + Template + ".txt", UriKind.Relative));

            using (StreamReader _StreamReader = new StreamReader(_StreamResourceInfo.Stream))
            {
                XamlString = _StreamReader.ReadToEnd();

                if (XamlString.Contains("x:Class=\"XAMLStudio.Xaml.class\""))
                {
                    XamlString = XamlString.Replace("x:Class=\"XAMLStudio.Xaml.class\"", "x:Class=\"XAMLStudio.Xaml." + Name + "\"");
                }

                _StreamReader.Close();
            }

            #endregion

            #region IMAGE
            using (IsolatedStorageFile MyStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string ThemeString = "Dark";

                if ((Visibility)Application.Current.Resources["PhoneLightThemeVisibility"] == System.Windows.Visibility.Visible)
                {
                    ThemeString = "Light";
                }

                BitmapImage TempBitmap = new BitmapImage();
                TempBitmap.SetSource(Application.GetResourceStream(new Uri("Images\\Pages\\" + ThemeString + "\\" + Template + ".jpg", UriKind.Relative)).Stream);
                WriteableBitmap TempWriteableBitmap = new WriteableBitmap(TempBitmap);

                if (MyStorage.FileExists("Shared/ShellContent/" + Name + ".jpg"))
                {
                    MyStorage.DeleteFile("Shared/ShellContent/" + Name + ".jpg");
                }

                IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream("Shared\\ShellContent\\" + Name + ".jpg", FileMode.OpenOrCreate, FileAccess.ReadWrite, MyStorage);
                Extensions.SaveJpeg(TempWriteableBitmap, fileStream, TempWriteableBitmap.PixelWidth, TempWriteableBitmap.PixelHeight, 0, 100);
                fileStream.Close();
            }
            #endregion

            Files.Add(Name, new XSFile
            {
                XAML = XamlString,
                FileName = Name,
                BaseTemplate = Template
            });
        }
        public static void DeleteFile(string Name)
        {
            if (FileExists(Name))
            {
                Files.Remove(Name);

                using (IsolatedStorageFile MyStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!MyStorage.FileExists("Shared/ShellContent/" + Name + ".jpg"))
                    {
                        MyStorage.DeleteFile("Shared/ShellContent/" + Name + ".jpg");
                    }
                }
            }
        }
        public static void ClearFiles()
        {
            using (IsolatedStorageFile MyStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                foreach (string s in MyStorage.GetFileNames("Shared/ShellContent/*"))
                {
                    DeleteFile(s);
                }
            }
            Files.Clear();
        }

        public static bool FileExists(string Name)
        {
            return Files.ContainsKey(Name);
        }
        public static bool CheckCharacters(string Name)
        {
            foreach (char c in Name.ToCharArray())
            {
                if (c == ' ')
                {
                    continue;
                }

                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class XSFile
    {
        public string FileName
        {
            get;
            set;
        }
        public string XAML
        {
            get;
            set;
        }
        public string BaseTemplate
        {
            get;
            set;
        }
        public BitmapImage ScreenShot
        {
            get
            {
                BitmapImage bitmapImage = new BitmapImage();

                using (IsolatedStorageFileStream fileStream = IsolatedStorageFile.GetUserStoreForApplication().OpenFile("Shared\\ShellContent\\" + FileName + ".jpg", FileMode.Open, FileAccess.Read))
                {
                    bitmapImage.SetSource(fileStream);
                }

                return bitmapImage;
            }
        }
    }
}