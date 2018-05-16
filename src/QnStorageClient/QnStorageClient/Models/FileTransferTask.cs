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