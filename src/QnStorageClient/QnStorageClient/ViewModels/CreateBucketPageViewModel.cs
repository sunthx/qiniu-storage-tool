using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QnStorageClient.Models;
using QnStorageClient.Services;
using QnStorageClient.Utils;

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
            NotificationService.ShowMessage(ResourceUtils.GetText("BucketCreating"));
            var created = await QiniuService.CreateBucket(BucketObject) && await QiniuService.SetBucketAccessControl(BucketObject.Name, BucketObject.Private != "0");
            NotificationService.Dismiss();

            NotificationService.ShowMessage(
                created ? ResourceUtils.GetText("BucketCreatedSucceed") : ResourceUtils.GetText("BucketCreatedFailed"),
                2000);
        }

        private void CancelCommandExecute()
        {
            NavigationService.GoBack();
        }
    }
}
