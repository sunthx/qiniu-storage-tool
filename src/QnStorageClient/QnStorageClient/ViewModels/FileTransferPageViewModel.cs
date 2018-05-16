using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace QnStorageClient.ViewModels
{
    public class FileTransferPageViewModel : ViewModelBase
    {
        public FileTransferPageViewModel()
        {
            Messenger.Default.Register<FileTransferTask>(this,AddTaskMessageAction);
            UploadTask = new ObservableCollection<FileTransferTask>();
            DownloadTask = new ObservableCollection<FileTransferTask>();
        }

        private void AddTaskMessageAction(FileTransferTask message)
        {
            if (message.TransferType == TransferType.Download)
            {
                DownloadTask.Add(message);
            }
            else
            {
                UploadTask.Add(message);
            }             
        }

        public ObservableCollection<FileTransferTask> UploadTask { get; set; }

        public ObservableCollection<FileTransferTask> DownloadTask { set; get; }

        public RelayCommand<FileTransferTask> DeleteTaskCommand { get; set; }
    }
} 