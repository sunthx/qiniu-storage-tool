using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace QnStorageClient.Models
{
    public class BucketObject : ViewModelBase
    {
        public BucketObject()
        {
            Region = "z0";
            Private = "0";
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private string _region;
        public string Region
        {
            get => _region;
            set => Set(ref _region, value);
        }

        private string _regionName;
        public string RegionName
        {
            get => _regionName;
            set => Set(ref _regionName, value);
        }

        private string _private;
        public string Private
        {
            get => _private;
            set => Set(ref _private, value);
        }

        private List<string> _domains;
        public List<string> Domains
        {
            get => _domains;
            set => Set(ref _domains, value);
        }
    }
}
