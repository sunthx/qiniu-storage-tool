using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QnStorageClient.Models
{
    public class ProgressArgs
    {
        public string TaskId { get; set; }
        public double? Progress { get; set; }
    }
}
