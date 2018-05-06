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
        public MainPageViewModel()
        {
            BucketListViewModel = new BucketListViewModel();
            SettingCommand = new RelayCommand(SettingCommandExecute);
        }

        public BucketListViewModel BucketListViewModel { get; set; }
        public RelayCommand SettingCommand { get; set; }
        private void SettingCommandExecute()
        {

        }
    }
}
