using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using QnStorageClient.Views;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using QnStorageClient.Annotations;
using QnStorageClient.Services;

namespace QnStorageClient
{
    public sealed partial class MainPage : INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            Buckets = new ObservableCollection<string>();
            Loaded += MainPage_Loaded;
        }

        public ObservableCollection<string> Buckets { get; set; }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _coreTitleBar.LayoutMetricsChanged += OnLayoutMetricsChanged;
            Window.Current.SizeChanged += OnWindowSizeChanged;
            UpdateLayoutMetrics();
        }

        private void SettingButtonClick(object sender, RoutedEventArgs e)
        {
            if (ContentFrame.Content is SettingPage)
            {
                return;
            }

            ContentFrame.Navigate(typeof(SettingPage));
        }

        private void BucketListItemClick(object sender, ItemClickEventArgs e)
        {
            var bucketName = e.ClickedItem.ToString();
            ContentFrame.Navigate(typeof(FileListPage), bucketName);
        }


        #region Title Bar

        private readonly CoreApplicationViewTitleBar _coreTitleBar = CoreApplication.GetCurrentView().TitleBar;

        public Thickness CoreTitleBarPadding
        {
            get
            {
                if (ApplicationView.GetForCurrentView().IsFullScreenMode)
                {
                    return new Thickness(0, 0, 0, 0);
                }
                return FlowDirection == FlowDirection.LeftToRight ?
                    new Thickness { Left = _coreTitleBar.SystemOverlayLeftInset, Right = _coreTitleBar.SystemOverlayRightInset } :
                    new Thickness { Left = _coreTitleBar.SystemOverlayRightInset, Right = _coreTitleBar.SystemOverlayLeftInset };
            }
        }

        public double CoreTitleBarHeight => _coreTitleBar.Height;

        private void OnWindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            UpdateLayoutMetrics();
        }

        void OnLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object e)
        {
            UpdateLayoutMetrics();
        }

        void UpdateLayoutMetrics()
        {
            OnPropertyChanged(nameof(CoreTitleBarHeight));
            OnPropertyChanged(nameof(CoreTitleBarPadding));
        }

        #endregion

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(TitleBarBackgroundElement);

            //load data
            var setting = AppSettingService.GetSetting();
            if (string.IsNullOrEmpty(setting.Ak) || string.IsNullOrEmpty(setting.Sk))
            {
                ContentFrame.Navigate(typeof(SettingPage));
            }
            else
            {
                QiniuService.Initialize(setting.Ak,setting.Sk);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
