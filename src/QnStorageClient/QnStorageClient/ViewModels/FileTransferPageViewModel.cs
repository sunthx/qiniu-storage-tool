using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QnStorageClient.Models;
using QnStorageClient.Services;

namespace QnStorageClient.ViewModels
{
    public class FileTransferPageViewModel : ViewModelBase
    {
        public FileTransferPageViewModel()
        {
            UploadTask = new ObservableCollection<FileTransferTask>();
            DownloadTask = new ObservableCollection<FileTransferTask>();

            DeleteTaskCommand = new RelayCommand<FileTransferTask>(DeleteTaskCommandExecute);
            SuspendTaskCommand = new RelayCommand<FileTransferTask>(SuspendTaskCommandExecute);
        }

        public void Initialize()
        {
            var transferTasks = FileTransferService.GetAllTransferTask();
            transferTasks.ForEach(item =>
            {
                if (item.TransferType == TransferType.Download)
                {
                    DownloadTask.Add(item);
                }
                else
                {
                    UploadTask.Add(item);
                }
            });
        }

        public ObservableCollection<FileTransferTask> UploadTask { get; set; }
        public ObservableCollection<FileTransferTask> DownloadTask { set; get; }
        public RelayCommand<FileTransferTask> DeleteTaskCommand { get; set; }
        public RelayCommand<FileTransferTask> SuspendTaskCommand { get; set; }

        private void SuspendTaskCommandExecute(FileTransferTask fileTransferTasks)
        {
            fileTransferTasks.TransferState = TransferState.Suspended;
        }

        private void DeleteTaskCommandExecute(FileTransferTask fileTransferTask)
        {
            fileTransferTask.TransferState = TransferState.Aborted;
        }
    }
} 