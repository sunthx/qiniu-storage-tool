using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qiniu.Share.Storage;
using Qiniu.Share.Util;

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

        public static async Task<List<string>> GetBuckets()
        {
            var bucketManager = new BucketManager(_currentMac, _config);
            var queryResult = await Task.Factory.StartNew(() => bucketManager.Buckets(true));
            if (queryResult?.Result == null)
            {
                return new List<string>();
            }

            return queryResult.Result;
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
    }
}
