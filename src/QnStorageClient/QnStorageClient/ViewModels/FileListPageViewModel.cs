using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp;
using Qiniu.Share.Storage;
using QnStorageClient.Models;
using QnStorageClient.Services;

namespace QnStorageClient.ViewModels
{
    public class FileListPageViewModel : ViewModelBase
    {                                      
        public FileListPageViewModel()
        {
            UploadFileCommand = new RelayCommand(UploadFileCommandExecute);
            RefreshFileListCommand = new RelayCommand(RefreshFileListCommandExecute);
            DeleteFileCommand = new RelayCommand(DeleteFileCommandExecute);
            DownloadFileCommand = new RelayCommand(DownloadFileCommandExecute);
        }

        private BucketObject _currentBucketInfo;
        public BucketObject CurrentBucketInfo
        {
            get => _currentBucketInfo;
            set => Set(ref _currentBucketInfo, value);
        }

        private IncrementalLoadingCollection<FileListSource, FileItemViewModel> _fileItems;
        public IncrementalLoadingCollection<FileListSource, FileItemViewModel> FileItems
        {
            set => Set(ref _fileItems, value);
            get => _fileItems;
        }

        public RelayCommand UploadFileCommand { get; set; }

        public RelayCommand RefreshFileListCommand { get; set; }

        public RelayCommand DeleteFileCommand { get; set; }

        public RelayCommand DownloadFileCommand { get; set; }

        public async Task LoadFiles(BucketObject bucketInfo)
        {
            CurrentBucketInfo = bucketInfo;
            CurrentBucketInfo.RegionName = await GetZoneName(bucketInfo.Name);
            FileItems = new IncrementalLoadingCollection<FileListSource, FileItemViewModel>(new FileListSource(bucketInfo.Name));
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

        private void UploadFileCommandExecute()
        {
        }

        private void RefreshFileListCommandExecute()
        {
        }

        private void DeleteFileCommandExecute()
        {
        }

        private void DownloadFileCommandExecute()
        {
        }
    }
}