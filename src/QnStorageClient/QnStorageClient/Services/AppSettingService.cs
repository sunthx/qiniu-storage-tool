using Windows.Storage;
using QnStorageClient.Models;

namespace QnStorageClient.Services
{
    public static class AppSettingService
    {
        private static string _ak = "AccessKey";
        private static string _sk = "SecretKey";
        private static string _storagePath = "StoragePath";

        public static AppSetting GetSetting()
        {
            var setting = new AppSetting();
            var localSettings = ApplicationData.Current.LocalSettings;
            setting.Ak = localSettings.Values[_ak]?.ToString();
            setting.Sk = localSettings.Values[_sk]?.ToString();
            setting.StoragePath = localSettings.Values[_storagePath]?.ToString();
            return setting;
        }

        public static void SaveSetting(AppSetting setting)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[_ak] = setting.Ak;
            localSettings.Values[_sk] = setting.Sk;
            localSettings.Values[_storagePath] = setting.StoragePath;
        }
    }
}
