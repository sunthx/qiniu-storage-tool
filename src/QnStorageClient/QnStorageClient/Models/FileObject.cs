using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QnStorageClient.Models
{
    public class FileObject
    {
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public long FileSize { set; get; }
        public long PutTime { set; get; }
        public int FileType { set; get; }
    }

    public class FileType
    {

    }


}
