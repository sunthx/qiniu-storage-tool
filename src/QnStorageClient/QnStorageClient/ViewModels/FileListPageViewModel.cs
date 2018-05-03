using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace QnStorageClient.ViewModels
{
    public class FileListPageViewModel : ViewModelBase
    {
        public RelayCommand UploadFileCommand { get; set; }

        public RelayCommand RefreshFileListCommand { get; set; }

        public RelayCommand DeleteFileCommand { get; set; }

        public RelayCommand DownloadFileCommand { get; set; }
    }
}
