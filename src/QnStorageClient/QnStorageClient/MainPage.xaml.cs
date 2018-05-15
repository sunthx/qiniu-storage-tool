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
using Microsoft.Toolkit.Uwp.UI.Controls;
using QnStorageClient.Annotations;
using QnStorageClient.Models;
using QnStorageClient.Services;
using QnStorageClient.ViewModels;

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

            //notification
            NotificationService.Initialize(AppNotification);

            //main page datacontext
            var dataContext = new MainPageViewModel();
            DataContext = dataContext;
            
            //NaviagtionService Initialize
            NavigationService.MainFrame = ContentFrame;
            NavigationService.RegisterPageType("setting",typeof(SettingPage));
            NavigationService.RegisterPageType("files",typeof(FileListPage));
            NavigationService.RegisterPageType("create", typeof(CreateBucketPage));

            //load data
            var setting = AppSettingService.GetSetting();
            if (string.IsNullOrEmpty(setting.Ak) || string.IsNullOrEmpty(setting.Sk))
            {
                NavigationService.NaviageTo("setting");
            }
            else
            {
                QiniuService.Initialize(setting.Ak,setting.Sk);
                await dataContext.BucketListViewModel.Initialize();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AppNotification_OnClosed(object sender, InAppNotificationClosedEventArgs e)
        {
            LoadingGrid.Visibility = Visibility.Collapsed;
        }

        private void AppNotification_OnOpening(object sender, InAppNotificationOpeningEventArgs e)
        {
            LoadingGrid.Visibility = Visibility.Visible;
        }
    }
}
