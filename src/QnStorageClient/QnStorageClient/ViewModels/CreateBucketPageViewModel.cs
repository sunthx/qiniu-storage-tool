using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QnStorageClient.Models;
using QnStorageClient.Services;

namespace QnStorageClient.ViewModels
{
    public class CreateBucketPageViewModel : ViewModelBase
    {
        public BucketObject BucketObject { get; set; }

        public RelayCommand CreateCommand { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public CreateBucketPageViewModel()
        {
            BucketObject = new BucketObject();
            CreateCommand = new RelayCommand(async ()=> await CreateCommandExecute());
            CancelCommand = new RelayCommand(CancelCommandExecute);
        }

        private async Task CreateCommandExecute()
        {
            await QiniuService.CreateBucket(BucketObject);
            await QiniuService.SetBucketAccessControl(BucketObject.Name, BucketObject.Private != "0");
        }

        private void CancelCommandExecute()
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }
    }
}
