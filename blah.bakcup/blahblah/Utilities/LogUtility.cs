using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Runtime.CompilerServices;
using System.IO;

namespace blahblah.Utilities
{
    class LogUtility
    {
        private static readonly Object _syncObject = new Object();

        public static ILog GetLogger([CallerFilePath]String filename = "")
        {
            return LogManager.GetLogger(filename);
        }

        public static void LogIt(String logMessage, TextWriter writer)
        {
            lock(_syncObject)
            {
                writer.WriteLine(logMessage);
                writer.Flush();
            }
        }
    }
}
