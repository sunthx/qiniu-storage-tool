using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QnStorageClient.Models;
using QnStorageClient.Services;

namespace QnStorageClient.ViewModels
{
    public class BucketListViewModel : ViewModelBase
    {
        public RelayCommand AddBucketCommand { get; set; }

        public RelayCommand RefreshBucketListCommand { get; set; }

        public ObservableCollection<BucketObject> Buckets { get; set; }

        private BucketObject _currentSelectedBucketObject;
        public BucketObject CurrentSelectedBucketObject
        {
            get => _currentSelectedBucketObject;
            set => Set(ref _currentSelectedBucketObject, value);
        }

        public BucketListViewModel()
        {
            Buckets = new ObservableCollection<BucketObject>();
            AddBucketCommand = new RelayCommand(AddBucketCommandExecute);
            RefreshBucketListCommand = new RelayCommand(async()=> await RefreshBucketListCommandExecute());
        }

        public async Task Initialize()
        {
            await RefreshBucketListCommandExecute();
        }

        private void AddBucketCommandExecute()
        {

        }

        private async Task RefreshBucketListCommandExecute()
        {
            if (Buckets.Any())
            {
                Buckets.Clear();
            }

            var queryResult = await QiniuService.GetBuckets();
            if (!queryResult.Any())
            {
                return;
            }

            queryResult.ForEach(item =>
            {
                Buckets.Add(new BucketObject { Name = item });
            });
        }
    }
}
