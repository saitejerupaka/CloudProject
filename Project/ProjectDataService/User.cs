using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectDataService
{
    public class User
    {
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BucketName { get; set; }

        public User()
        {
        }

        public User(string email, string fname, string lname, string bucketname)
        {
            UserEmail = email;
            FirstName = fname;
            LastName = lname;
            BucketName = bucketname;
        }
    }
}
