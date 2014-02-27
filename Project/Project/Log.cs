using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Project
{
    public class Log
    {
        public string Message { get; set; }
        public Log()
        {
            
        }

        public void WriteLog(string logMessage,TextWriter writer)
        {
            writer.Write("\r\nLog Entry : ");
            writer.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            writer.WriteLine("  :");
            writer.WriteLine("  :{0}", logMessage);
            writer.WriteLine("-------------------------------");
        }
    }
}
