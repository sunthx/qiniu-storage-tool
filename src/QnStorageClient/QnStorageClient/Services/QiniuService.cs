using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Microsoft.Toolkit.Uwp.Helpers;
using Qiniu.Share.IO;
using Qiniu.Share.Storage;
using Qiniu.Share.Util;
using Qiniu.Uwp.FileStorage;
using QnStorageClient.Models;

namespace QnStorageClient.Services
{
    public static class QiniuService
    {
        static QiniuService()
        {
            IOUtils.Api = new UwpFileApi();
        }

        private static Mac _currentMac;
        private static readonly Config _config = new Config();

        public static void Initialize(string ak, string sk)
        {
            _currentMac = new Mac(ak, sk);
        }

        public static async Task<Zone> GetBucketZoneInfo(string bucketName)
        {
            return await Task.Factory.StartNew(() =>
            {
#if DEBUG
                return new Zone {ApiHost = Zone.ZONE_AS_Singapore.ApiHost};
#else
                return ZoneHelper.QueryZone(_currentMac.AccessKey, bucketName);
#endif
            });
        }

        public static async Task<List<string>> GetBuckets(bool isShare = true)
        {
#if DEBUG
            return new List<string>
            {
                "Bucket1",
                "Bucket2",
                "Bucket3"
            };
#else
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() =>
            {
                var allBuckets = bucketManager.Buckets(true);
                return allBuckets;
            });

            if (queryResult?.Result == null)
                return new List<string>();

            return queryResult.Result;
#endif
        }

        public static async Task<bool> DeleteFile(string bucketName, string fileId)
        {
#if DEBUG
            return await Task.FromResult(true);
#else
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() => bucketManager.Delete(bucketName, fileId));
            return queryResult.Code == 200;
#endif
        }

        public static async Task<ListInfo> GetFiles(
            string bucketName,
            string marker = null,
            string prefix = null,
            int limit = 100,
            string delimiter = null)
        {
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() =>
                bucketManager.ListFiles(bucketName, prefix, marker, limit, delimiter));
            return queryResult.Result;
        }

        public static async Task<bool> CreateBucket(BucketObject bucketObject)
        {
#if DEBUG
            return await Task.FromResult(true);
#else
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult =
                await Task.Factory.StartNew(() => bucketManager.Create(bucketObject.Name, bucketObject.Region));
            return queryResult.Code == 200;
#endif
        }

        public static async Task<bool> SetBucketAccessControl(string bucketName, bool isPrivate)
        {
#if DEBUG
            return await Task.FromResult(true);
#else
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() => bucketManager.SetAccessControl(bucketName, isPrivate));
            return queryResult.Code == 200;
#endif
        }

        public static async Task<List<string>> Domains(string bucketName)
        {
#if DEBUG
            return await Task.FromResult(new List<string>
            {
                "www.domain1.com",
                "www.domain2.com"
            });
#else
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() => bucketManager.Domains(bucketName));
            if (queryResult.Code == 200)
                return queryResult.Result;

            return new List<string>();
#endif
        }

        public static string CreateResourcePublicUrl(string domians, string resouceId)
        {
#if DEBUG
            return "www.domain1.com";
#else
            return DownloadManager.CreatePublishUrl(domians, resouceId);
#endif
        }

        public static Task<bool> DownloadFile(FileTransferTask task)
        {
#if DEBUG
            return Task.FromResult(true);
#else
            return Task.Factory.StartNew(() =>
            {
                var result = DownloadManager.Download(task.FileObject.PublicUrl,
                    Path.Combine(AppSettingService.GetSetting().StoragePath, task.FileObject.FileName));

                return result.Code == 200;
            });
#endif
        }

        public static async Task<bool> UploadFile(FileTransferTask task)
        {
            var resumeDirectory = Path.Combine(ApplicationData.Current.TemporaryFolder.Path, "upload", "resume");
            if (!Directory.Exists(resumeDirectory))
            {
                Directory.CreateDirectory(resumeDirectory);
            }

            var resumeRecordFilePath = Path.Combine(resumeDirectory, task.FileObject.FileName);
            var tempFolder = await StorageFolder.GetFolderFromPathAsync(resumeDirectory);
            var resumeRecordFile = await tempFolder.TryGetItemAsync(task.FileObject.FileName);
            if (resumeRecordFile == null)
            {
                await tempFolder.CreateFileAsync(task.FileObject.FileName, CreationCollisionOption.ReplaceExisting);
            }
            
            return await Task.Factory.StartNew(async () =>
            {
                void ProgressHandler(long uploadBytes, long totalBytes)
                {
                    var percent = uploadBytes * 1.0 / totalBytes;

                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        task.Progress = percent;
                    });
                }

#if DEBUG
                int total = 100;
                for (int i = 0; i < 100; i++)
                {
                    ProgressHandler(i,total);
                    await Task.Delay(1000);
                }

                return true;

#else
                var uploadManager = new UploadManager(_config);
                var putExtra = new PutExtra
                {
                    ResumeRecordFile = resumeRecordFilePath,
                    BlockUploadThreads = 1,
                    ProgressHandler = ProgressHandler,
                    UploadController = delegate
                    {
                        if (task.TransferState == TransferState.Aborted)
                            return UploadControllerAction.Aborted;

                        return task.TransferState == TransferState.Suspended
                            ? UploadControllerAction.Suspended
                            : UploadControllerAction.Activated;
                    }
                };

                var putPolicy = new PutPolicy
                {
                    Scope = task.BucketObject.Name
                };
                putPolicy.SetExpires(24 * 30 * 3600);

                var auth = new Auth(_currentMac);
                var uptoken = auth.CreateUploadToken(putPolicy.ToJsonString());

                var fileStorage = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(task.FileObject.FileName);
                var currentFileStream = await fileStorage.OpenStreamForReadAsync();
                var uploadResult = uploadManager.UploadStream(
                    currentFileStream,
                    task.FileObject.FileName,
                    uptoken,
                    putExtra);

                return uploadResult.Code == 200;
#endif
            }).Unwrap();
        }

        public static Task<bool> CheckRemoteDuplicate(string currentBucketName, string fileKey)
        {
#if DEBUG
            return Task.FromResult(true);
#else
            return Task.Factory.StartNew(() =>
            {
                var bucketManager = new BucketManager(_currentMac, _config);
                var statResult = bucketManager.Stat(currentBucketName, fileKey);
                return !string.IsNullOrEmpty(statResult?.Result.Hash);
            });
#endif
        }
    }
}