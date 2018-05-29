using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QnStorageClient.Services;

namespace QnStorageClient.ViewModels
{
    public class SettingPageViewModel : ViewModelBase
    {
        private string _ak;
        public string Ak
        {
            get => _ak;
            set => Set(ref _ak, value);
        }

        private string _sk;
        public string Sk
        {
            get => _sk;
            set => Set(ref _sk, value);
        }

        private string _storagePath;
        public string StoragePath
        {
            get => _storagePath;
            set => Set(ref _storagePath, value);
        }

        public RelayCommand UpdateAccountCommand { get; set; }

        public RelayCommand SelectStoragePathCommand { get; set; }

        public SettingPageViewModel()
        {
            UpdateAccountCommand = new RelayCommand(UpdateAccountCommandExecute);
            SelectStoragePathCommand = new RelayCommand(async ()=> await SelectStoragePathCommandExecute());
        }

        private void UpdateAccountCommandExecute()
        {
            var setting = AppSettingService.GetSetting();
            setting.Ak = Ak;
            setting.Sk = Sk;

            AppSettingService.SaveSetting(setting);
        }

        private async Task SelectStoragePathCommandExecute()
        {
            var folderPicker = new Windows.Storage.Pickers.FolderPicker
            {
                SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop
            };
            folderPicker.FileTypeFilter.Add("*");

            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                var setting = AppSettingService.GetSetting();
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.AddOrReplace(setting.StorageToken, folder);
                StoragePath = folder.Path;
                setting.StoragePath = StoragePath;
                AppSettingService.SaveSetting(setting);
            }
        }
    }
}
