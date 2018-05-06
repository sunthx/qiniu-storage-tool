using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using QnStorageClient.Models;

namespace QnStorageClient.ViewModels
{
    public class FileItemViewModel : ViewModelBase
    {
        public FileObject FileObject { get; set; }

        private bool _isChecked;
        public bool IsChecked
        {
            set => Set(ref _isChecked, value);
            get => _isChecked;
        }
    }
}
