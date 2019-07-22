using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace AfonsoftIPTV.Helpers
{
    public static class PlayerSettings
    {
        static ISettings AppSettings => CrossSettings.Current;
        public static void ClearAllData()
        {
            AppSettings.Clear();
        }

        public static string PathFile
        {
            get => AppSettings.GetValueOrDefault(nameof(PathFile), "/storage/emulated/0/Movies/lista.m3u");
            set => AppSettings.AddOrUpdateValue(nameof(PathFile), value);
        }

        public static string UrlFile
        {
            get => AppSettings.GetValueOrDefault(nameof(UrlFile), "https://gist.githubusercontent.com/Gunboybr/fc6715423ff24db7ff046bd43166d1a1/raw/desenhosmegabox");
            set => AppSettings.AddOrUpdateValue(nameof(UrlFile), value);
        }      

        public static string License
        {
            get => AppSettings.GetValueOrDefault(nameof(License), null);
            set => AppSettings.AddOrUpdateValue(nameof(License), value);
        }

        public static bool SwitchLogo
        {
            get => AppSettings.GetValueOrDefault(nameof(SwitchLogo), true);
            set => AppSettings.AddOrUpdateValue(nameof(SwitchLogo), value);
        }
        public static bool SwitchUrl
        {
            get => AppSettings.GetValueOrDefault(nameof(SwitchUrl), true);
            set => AppSettings.AddOrUpdateValue(nameof(SwitchUrl), value);
        }
        public static bool SwitchFolder
        {
            get => AppSettings.GetValueOrDefault(nameof(SwitchFolder), true);
            set => AppSettings.AddOrUpdateValue(nameof(SwitchFolder), value);
        }
    }
}