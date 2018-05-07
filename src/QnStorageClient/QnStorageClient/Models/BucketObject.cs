using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QnStorageClient.Models
{
    public class BucketObject
    {
        public BucketObject()
        {
            Region = "z0";
            Private = "0";
        }
        public string Name { get; set; }

        public string Region  { get; set; }

        public string Private { get; set; }
    }
}
