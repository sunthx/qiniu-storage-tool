using System;
using GalaSoft.MvvmLight;

namespace QnStorageClient.Models
{
    public class FileTransferTask : ViewModelBase
    {
        public FileTransferTask(FileObject fileObject)
        {
            TaskId = Guid.NewGuid().ToString();
            FileObject = fileObject;
        }

        /// <summary>
        /// 任务编号
        /// </summary>
        public string TaskId { get; set; }

        /// <summary>
        /// 文件信息
        /// </summary>
        public FileObject FileObject { get; set; }

        /// <summary>
        /// Bucket信息
        /// </summary>
        public BucketObject BucketObject { get; set; }

        /// <summary>
        /// 传输类型
        /// </summary>
        public TransferType TransferType { get; set; }
                
        /// <summary>
        /// 任务状态
        /// </summary>
        public TransferState TransferState { get; set; }

        private double _progress;
        /// <summary>
        /// 进度
        /// </summary>
        public double Progress
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

    public enum TransferState
    {
        Idle,
        Transfering,
        Aborted,
        Suspended,
        Activated
    }
}