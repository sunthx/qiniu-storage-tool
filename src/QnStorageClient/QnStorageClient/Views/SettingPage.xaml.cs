using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using QnStorageClient.Services;
using QnStorageClient.ViewModels;

namespace QnStorageClient.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingPage : Page
    {
        public SettingPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var setting = AppSettingService.GetSetting();
            var settingPageViewModel = new SettingPageViewModel
            {
                Ak = setting.Ak,
                Sk = setting.Sk,
                StoragePath = setting.StoragePath
            };

            DataContext = settingPageViewModel;
        }
    }
}
