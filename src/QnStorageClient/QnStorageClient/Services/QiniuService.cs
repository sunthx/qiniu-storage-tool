using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Qiniu.Share.Http;
using Qiniu.Share.Storage;
using Qiniu.Share.Util;
using QnStorageClient.Models;

namespace QnStorageClient.Services
{
    public static class QiniuService
    {
        private static Mac _currentMac;
        private static Config _config = new Config();

        public static void Initialize(string ak,string sk)
        {
            _currentMac = new Mac(ak,sk);
        }

        public static async Task<Zone> GetBucketZoneInfo(string bucketName)
        {
            return await Task.Factory.StartNew(() => ZoneHelper.QueryZone(_currentMac.AccessKey, bucketName));
        }

        public static async Task<List<string>> GetBuckets(bool isShare = true)
        {
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() =>
            {
                var allBuckets = bucketManager.Buckets(true);
                return allBuckets;
            });

            if (queryResult?.Result == null)
            {
                return new List<string>();
            }

            return queryResult.Result;
        }

        public static async Task<bool> DeleteFile(string bucketName,string fileId)
        {
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() => bucketManager.Delete(bucketName, fileId));
            return queryResult.Code == 200;
        }

        public static async Task<ListInfo> GetFiles(
            string bucketName, 
            string marker = null,
            string prefix = null,
            int limit = 100,
            string delimiter = null)
        {
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() => bucketManager.ListFiles(bucketName, prefix, marker, limit, delimiter));
            return queryResult.Result;
        }

        public static async Task<bool> CreateBucket(BucketObject bucketObject)
        {
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() => bucketManager.Create(bucketObject.Name, bucketObject.Region));
            return queryResult.Code == 200;
        }

        public static async Task<bool> SetBucketAccessControl(string bucketName,bool isPrivate)
        {
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() => bucketManager.SetAccessControl(bucketName, isPrivate));
            return queryResult.Code == 200;
        }

        public static async Task<List<string>> Domains(string bucketName)
        {
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() => bucketManager.Domains(bucketName));
            if (queryResult.Code == 200)
            {
                return queryResult.Result;
            }

            return new List<string>();
        }

        public static string CreateResourcePublicUrl(string domians,string resouceId)
        {
            return DownloadManager.CreatePublishUrl(domians, resouceId);
        }

        public static Task<bool> DownloadFile(FileTransferTask task)
        {
            return Task.Factory.StartNew(() =>
            {
                var result = DownloadManager.Download(task.FileObject.PublicUrl,
                    Path.Combine(AppSettingService.GetSetting().StoragePath, task.FileObject.FileName));

                return result.Code == 200;
            });
           
        }

        public static Task<bool> UploadFile(FileTransferTask task)
        {
            var resumeDirectory = Path.Combine(ApplicationData.Current.TemporaryFolder.Path,"upload","resume");
            if (!Directory.Exists(resumeDirectory))
            {
                Directory.CreateDirectory(resumeDirectory);
            }

            var resumeRecordFilePath = Path.Combine(resumeDirectory,task.FileObject.FileName);
            return Task.Factory.StartNew(() =>
            {
                var uploadManager = new UploadManager(_config);
                void ProgressHandler(long uploadBytes, long totalBytes)
                {
                    double percent = uploadBytes * 1.0 / totalBytes;
                    task.Progress = percent;
                }

                var putExtra = new PutExtra
                {
                    ResumeRecordFile = resumeRecordFilePath,
                    BlockUploadThreads = 1,
                    ProgressHandler = ProgressHandler,
                    UploadController = delegate
                    {
                        if (task.TransferState == TransferState.Aborted)
                        {
                            return UploadControllerAction.Aborted;
                        }

                        return task.TransferState == TransferState.Suspended ? 
                            UploadControllerAction.Suspended:
                            UploadControllerAction.Activated;
                    }
                };

                var putPolicy = new PutPolicy
                {
                    Scope = task.BucketObject.Name
                };
                putPolicy.SetExpires(24 * 30 * 3600);

                var auth = new Auth(_currentMac);
                var uptoken = auth.CreateUploadToken(putPolicy.ToJsonString());
                var uploadResult = uploadManager.UploadFile(
                    task.FileObject.LocalPath, 
                    task.FileObject.FileName, 
                    uptoken, 
                    putExtra);

                return uploadResult.Code == 200;
            });
        }

        public static Task<bool> CheckRemoteDuplicate(string currentBucketName,string fileKey)
        {
            return Task.Factory.StartNew(() =>
            {
                var bucketManager = new BucketManager(_currentMac, _config);
                var statResult = bucketManager.Stat(currentBucketName, fileKey);
                return !string.IsNullOrEmpty(statResult?.Result.Hash);
            });
        }
    }
}
