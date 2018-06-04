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
            AllTasks = new ObservableCollection<FileTransferTask>();
            DeleteTaskCommand = new RelayCommand<FileTransferTask>(DeleteTaskCommandExecute);
            SuspendTaskCommand = new RelayCommand<FileTransferTask>(SuspendTaskCommandExecute);
            FilterCommand = new RelayCommand<string>(FilterCommandExecute);
        }

        public void Initialize()
        {
            var transferTasks = FileTransferService.GetAllTransferTask();
            transferTasks.ForEach(item =>
            {
                AllTasks.Add(item);
            });
        }

        public RelayCommand<string> FilterCommand { set; get; }
        public ObservableCollection<FileTransferTask> AllTasks { get; set; }
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

        private void FilterCommandExecute(string tag)
        {
            AllTasks.Clear();
            var transferTasks = FileTransferService.GetAllTransferTask();

            switch (tag)
            {
                case null:
                    transferTasks.ForEach(item =>
                    {
                        AllTasks.Add(item);
                    });
                    break;
                case "download":
                    transferTasks.ForEach(item =>
                    {
                        if(item.TransferType == TransferType.Download)
                            AllTasks.Add(item);
                    });
                    break;
                case "upload":
                    transferTasks.ForEach(item =>
                    {
                        if (item.TransferType == TransferType.Upload)
                            AllTasks.Add(item);
                    });
                    break;
            }
        }
    }
} 