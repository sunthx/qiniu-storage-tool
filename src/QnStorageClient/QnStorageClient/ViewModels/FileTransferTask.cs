using GalaSoft.MvvmLight;

namespace QnStorageClient.ViewModels
{
    public class FileTransferTask : ViewModelBase
    {
        /// <summary>
        /// 任务编号
        /// </summary>
        public string TaskId { get; set; }
        /// <summary>
        /// 传输类型
        /// </summary>
        public TransferType TransferType { get; set; }

        private double? _progress;
        /// <summary>
        /// 进度
        /// </summary>
        public double? Progress
        {
            set => Set(ref _progress, value);
            get => _progress;
        }
    }

    public enum TransferType
    {
        Upload,
        Download
    }
}