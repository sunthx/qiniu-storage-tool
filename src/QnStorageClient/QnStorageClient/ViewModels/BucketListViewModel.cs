using System.Collections.ObjectModel;
using System.Linq;
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
            set
            {
                Set(ref _currentSelectedBucketObject, value);
                if (_currentSelectedBucketObject != null)
                {
                    NavigationService.NaviageTo("files", _currentSelectedBucketObject);
                }
            }
        }                           

    public BucketListViewModel()
        {
            Buckets = new ObservableCollection<BucketObject>();
            AddBucketCommand = new RelayCommand(AddBucketCommandExecute);
            RefreshBucketListCommand = new RelayCommand(async () =>
            {
                NotificationService.ShowMessage("Loading");
                await RefreshBucketListCommandExecute();
                NotificationService.Dismiss();
            });
        }

        public async Task Initialize()
        {
            NotificationService.ShowMessage("Loading");
            await RefreshBucketListCommandExecute();
            NotificationService.Dismiss();
        }

        private void AddBucketCommandExecute()
        {
              NavigationService.NaviageTo("create");
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

            CurrentSelectedBucketObject = Buckets.First();
        }
    }
}
