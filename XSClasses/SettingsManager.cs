using System.IO.IsolatedStorage;

namespace XSClasses
{
    public static class SettingsManager
    {
        private static IsolatedStorageSettings SettingsFile = IsolatedStorageSettings.ApplicationSettings;

        public static void SetEntry(string ID, object NewValue)
        {
            if (SettingsFile.Contains(ID))
            {
                SettingsFile[ID] = NewValue;
                SettingsFile.Save();
            }
            else
            {
                SettingsFile.Add(ID, NewValue);
                SettingsFile.Save();
            }
        }
        public static object GetEntry(string ID, object DefaultValue)
        {
            if (!SettingsFile.Contains(ID))
            {
                SettingsFile.Add(ID, DefaultValue);
                SettingsFile.Save();
            }
            return SettingsFile[ID];
        }
    }
}