using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWeb.DataService
{
    public class FileMetaData
    {
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string Email_User { get; set; }
        public string FilePath { get; set; }
        public DateTime LastModified { get; set; }
    }
}