using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QnStorageClient.Models
{
    public class FileObject
    {
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public string MimeType { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { set; get; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public long PutTime { set; get; }
        /// <summary>
        /// 文件类型
        /// </summary>
        public int FileType { set; get; }
        /// <summary>
        /// 公开访问链接
        /// </summary>
        public string PublicUrl { set; get; }
    }
}
