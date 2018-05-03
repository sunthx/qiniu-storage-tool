using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace QnStorageClient.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public RelayCommand SettingCommand { get; set; }

        public RelayCommand AddBucketCommand { get; set; }

        public RelayCommand RefreshBucketListCommnad { get; set; }
    }
}
