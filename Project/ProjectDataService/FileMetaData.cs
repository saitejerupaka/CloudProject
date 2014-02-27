using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataService
{
    public class FileMetaData
    {
        public int FileID { get; set; }
        public string FileName { get; set; }
        public string Email_User { get; set; }
        public string FilePath { get; set; }
        public DateTime LastModified { get; set; }

        public FileMetaData()
        {
        }
        public FileMetaData(int fileId, string fileName, string email, string filePath, DateTime modified)
        {
            FileID = fileId;
            FileName = fileName;
            Email_User = email;
            FilePath = filePath;
            LastModified = modified;
        }
    }
}
