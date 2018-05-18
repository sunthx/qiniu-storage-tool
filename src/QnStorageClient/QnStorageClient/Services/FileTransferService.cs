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

        public static FileTransferTask AddTransferTask(FileTransferTask task)
        {
            lock (LockObject)
            {
                AllTask.Enqueue(task);
            }

            return task;
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
                            var transferTask = nextTask.TransferType == TransferType.Download ? 
                                DownloadFile(nextTask) : 
                                UploadFile(nextTask);
                            FileTransferTask.Add(nextTask.TaskId, transferTask);
                        }
                    }
                }

                Thread.Sleep(2000);
            }
        }

        private static async Task DownloadFile(FileTransferTask task)
        {
            await QiniuService.DownloadFile(task);
            RemoveTaskById(task.TaskId);
        }

        private static async Task UploadFile(FileTransferTask task)
        {
            if(await QiniuService.UploadFile(task))
            {
                RemoveTaskById(task.TaskId);
            }
        }

        private static void RemoveTaskById(string id)
        {
            lock (FileTransferTask)
            {
                if (FileTransferTask.ContainsKey(id))
                    FileTransferTask.Remove(id);
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