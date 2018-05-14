using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Collections;
using QnStorageClient.Models;
using QnStorageClient.Services;

namespace QnStorageClient.ViewModels
{
    public class FileListSource : IIncrementalSource<FileItemViewModel>
    {
        private readonly string _bucketName;
        private string _marker = null;
        public FileListSource(string bucketName)
        {
            _bucketName = bucketName;
        }

        public async Task<IEnumerable<FileItemViewModel>> GetPagedItemsAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = new CancellationToken())
        {
            var pageResult = new List<FileItemViewModel>();
            if (pageIndex != 0 && string.IsNullOrEmpty(_marker))
            {
                return pageResult;
            }

            var fetchResult = await QiniuService.GetFiles(_bucketName, _marker);
            _marker = fetchResult.Marker;
            if (fetchResult.Items == null || !fetchResult.Items.Any())
            {
                return pageResult;
            }

            fetchResult.Items.ForEach(file =>
            {
                var fileObject = new FileObject
                {
                    FileName = file.Key,
                    MimeType = file.MimeType,
                    FileSize = file.Fsize,
                    PutTime = file.PutTime
                };

                pageResult.Add(new FileItemViewModel
                {
                    FileObject = fileObject
                });
            });

            return pageResult;
        }
    }
}
