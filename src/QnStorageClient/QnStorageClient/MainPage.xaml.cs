using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using QnStorageClient.Views;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using Qiniu.Share.Storage;
using Qiniu.Share.Util;
using QnStorageClient.Annotations;
using QnStorageClient.Services;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace QnStorageClient
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : INotifyPropertyChanged
    {
        public MainPage()
        {
            InitializeComponent();
            Buckets = new ObservableCollection<string>();
            Loaded += MainPage_Loaded;
        }

        public ObservableCollection<string> Buckets { get; set; }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            _coreTitleBar.LayoutMetricsChanged += OnLayoutMetricsChanged;
            Window.Current.SizeChanged += OnWindowSizeChanged;
            UpdateLayoutMetrics();

            var queryResult = await QiniuService.GetBuckets();
            queryResult.ForEach(Buckets.Add);
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _coreTitleBar.ExtendViewIntoTitleBar = true;
            Window.Current.SetTitleBar(TitleBarBackgroundElement);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
