using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using QnStorageClient.Models;
using QnStorageClient.Services;

namespace QnStorageClient.ViewModels
{
    public class FileTransferPageViewModel : ViewModelBase
    {
        public FileTransferPageViewModel()
        {
            Messenger.Default.Register<NotificationMessage<FileObject>>(this,AddTaskMessageAction);
            UploadTask = new ObservableCollection<FileTransferTask>();
            DownloadTask = new ObservableCollection<FileTransferTask>();
        }

        private void AddTaskMessageAction(NotificationMessage<FileObject> message) 
        {
            if (message.Notification == "upload")
            {
                
            }
            else
            {
                var transferTask = FileTransferService.AddDownloadStack(message.Content);
                DownloadTask.Add(transferTask);
            }
        }

        public ObservableCollection<FileTransferTask> UploadTask { get; set; }

        public ObservableCollection<FileTransferTask> DownloadTask { set; get; }

        public RelayCommand<FileTransferTask> DeleteTaskCommand { get; set; }
    }
} 