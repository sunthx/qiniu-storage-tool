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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace QnStorageClient
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage
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
            var queryResult = await Task.Factory.StartNew(() =>
            {
                Mac mac = new Mac("", "");
                Config config = new Config();

                BucketManager bucketManager = new BucketManager(mac, config);
                return bucketManager.Buckets(true);
            });
        }

        private void SettingButtonClick(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(typeof(SettingPage));
        }
    }
}
