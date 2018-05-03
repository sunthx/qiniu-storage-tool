using System.ComponentModel;
using System.Runtime.CompilerServices;
using QnStorageClient.Annotations;

namespace QnStorageClient.ViewModels
{
    public class SettingPageViewModel : INotifyPropertyChanged
    {
        private string _ak;
        public string Ak
        {
            get => _ak;
            set { _ak = value; OnPropertyChanged(); }
        }

        private string _sk;
        public string Sk
        {
            get => _sk;
            set { _sk = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
