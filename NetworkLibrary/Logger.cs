using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace NetworkLibrary.Log
{
    public sealed class Logger
    {
        private static Logger _instance = null;
        private static FileStream logFile;
        private static readonly object objectLock = new object();
        
        private Logger()
        {
            if(logFile != null)
            {
                logFile = new FileStream("Log.txt", FileMode.Append);

            }
        }

        public static Logger Instance
        {
            get
            {
                lock(objectLock)
                {
                    if (_instance == null)
                    {
                        _instance = new Logger();
                    }
                    return _instance;
                }
            }
        }

        public void WriteLog(string text)
        {
            Console.WriteLine(text);
            var write = "[" + Stopwatch.GetTimestamp() + "] : "+ text ;
            if(logFile == null)
            {
                try
                {
                    logFile = new FileStream("Log.txt", FileMode.Append);
                    StreamWriter writer = new StreamWriter(logFile);
                    writer.Write(text);
                }
                catch
                {

                }
            }
            else
            {
                try
                {
                    StreamWriter writer = new StreamWriter(logFile);
                    writer.Write(text);
                }
                catch
                {

                }
            }
           
        }
    }
}
