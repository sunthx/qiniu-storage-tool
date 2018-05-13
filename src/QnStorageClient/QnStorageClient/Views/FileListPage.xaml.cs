using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Qiniu.Share.Storage;
using QnStorageClient.Annotations;
using QnStorageClient.Models;
using QnStorageClient.Services;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace QnStorageClient.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class FileListPage : INotifyPropertyChanged
    {
        public FileListPage()
        {
            InitializeComponent();
            Files = new ObservableCollection<FileObject>();
            DataContext = this;
        }

        public ObservableCollection<FileObject> Files { get; set; }

        private string _zoneName;
        public string ZoneName
        {
            get => _zoneName;
            set { _zoneName = value; OnPropertyChanged(); }
        }

        private string _bucketName;
        public string BucketName
        {
            get => _bucketName;
            set { _bucketName = value; OnPropertyChanged(); }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            var bucketName = e.Parameter?.ToString();
            BucketName = bucketName;
            ZoneName = await GetZoneName(bucketName);

            var files = await QiniuService.GetFiles(bucketName);
            files?.Items.ForEach(file =>
            {
                var fileObject = new FileObject
                {
                    FileName = file.Key,
                    MimeType = file.MimeType,
                    FileSize = file.Fsize,
                    PutTime = file.PutTime
                };        

                Files.Add(fileObject);
            });

            base.OnNavigatedTo(e);
        }

        private async Task<string> GetZoneName(string bucketName)
        {
            string zone;
            var zoneInfo = await QiniuService.GetBucketZoneInfo(bucketName);
            if (zoneInfo.ApiHost == Zone.ZONE_AS_Singapore.ApiHost)
            {
                zone = "新加坡";
            }
            else if (zoneInfo.ApiHost == Zone.ZONE_CN_East.ApiHost)
            {
                zone = "华东";
            }
            else if (zoneInfo.ApiHost == Zone.ZONE_CN_North.ApiHost)
            {
                zone = "华北";
            }
            else if (zoneInfo.ApiHost == Zone.ZONE_CN_South.ApiHost)
            {
                zone = "华南";
            }
            else if (zoneInfo.ApiHost == Zone.ZONE_US_North.ApiHost)
            {
                zone = "北美";
            }
            else
            {
                zone = "未知";
            }

            return zone;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
