using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using QnStorageClient.Models;

namespace QnStorageClient.Services
{
    public static class FileTransferService
    {
        private static readonly Queue<FileTransferTask> AllTask = new Queue<FileTransferTask>();
        private static readonly int _maxTaskCount = 10;
        private static readonly Dictionary<string, Task> FileTransferTask = new Dictionary<string, Task>();
        private static readonly object LockObject = new object();

        public static IProgress<ProgressArgs> Progress { get; set; }

        public static List<FileTransferTask> GetAllTransferTask()
        {
            lock (LockObject)
            {
                return AllTask.ToList();
            }
        }

        /// <summary>
        ///     添加下载任务
        /// </summary>
        /// <returns>返回任务编号</returns>
        public static FileTransferTask AddDownloadStack(FileObject fileObject)
        {
            var fileTransferTask = new FileTransferTask(fileObject)
            {
                TransferType = TransferType.Download
            };
            lock (LockObject)
            {
                AllTask.Enqueue(fileTransferTask);
            }

            return fileTransferTask;
        }

        public static void Start()
        {
            ThreadPool.QueueUserWorkItem(StartTransferLooping);
        }

        private static void StartTransferLooping(object param)
        {
            while (true)
            {
                lock (FileTransferTask)
                {
                    if (FileTransferTask.Count < _maxTaskCount)
                    {
                        var nextTask = GetNextTask();
                        if (nextTask != null)
                        {
                            var transferTask = DownloadFile(nextTask);
                            FileTransferTask.Add(nextTask.TaskId, transferTask);
                        }
                    }
                }
                Thread.Sleep(2000);
            }
        }

        private static Task DownloadFile(FileTransferTask task)
        {
            return Task.Factory.StartNew(() =>
            {
                QiniuService.DownloadFile(task);
                RemoveTaskById(task.TaskId);
            });

        }

        private static void RemoveTaskById(string id)
        {
            lock (FileTransferTask)
            {
                if (FileTransferTask.ContainsKey(id))
                {
                    FileTransferTask.Remove(id);
                }
            }
        }

        private static FileTransferTask GetNextTask()
        {
            lock (AllTask)
            {
                if (AllTask.TryDequeue(out var nextTask))
                    return nextTask;

                return null;
            }
        }
    }
}