using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Qiniu.Share.Storage;
using QnStorageClient.Annotations;
using QnStorageClient.Models;
using QnStorageClient.Services;
using QnStorageClient.ViewModels;

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
        }

        private FileListPageViewModel _fileListPageViewModel;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_fileListPageViewModel == null)
            {
                _fileListPageViewModel = new FileListPageViewModel();
                DataContext = _fileListPageViewModel;
            }

            _fileListPageViewModel.LoadFiles(e.Parameter as BucketObject);
            base.OnNavigatedTo(e);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
