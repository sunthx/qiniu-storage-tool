using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

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

            FileItems = new ObservableCollection<FileItemViewModel>();
        }

        public ObservableCollection<FileItemViewModel> FileItems { get; set; }

        public RelayCommand UploadFileCommand { get; set; }

        public RelayCommand RefreshFileListCommand { get; set; }

        public RelayCommand DeleteFileCommand { get; set; }

        public RelayCommand DownloadFileCommand { get; set; }

        public Task LoadFiles(string bucketName)
        {
            return Task.CompletedTask;
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