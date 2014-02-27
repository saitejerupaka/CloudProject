using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectWeb.DataService
{
    public class User
    {
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BucketName { get; set; }
    }
}