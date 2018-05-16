using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QnStorageClient.Models;

namespace QnStorageClient.Services
{
    public static class FileTransferService
    {
        private static Dictionary<string, FileObject> _uploadStack = new Dictionary<string, FileObject>();
        private static Dictionary<string,FileObject> _downloadStack = new Dictionary<string, FileObject>();

        public static void AddUploadStack()
        {
            
        }

        public static void AddDownloadStack()
        {

        }
    }
}
