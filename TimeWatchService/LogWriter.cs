using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace TimeWatchService
{
    public class LogWriter
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public static void WriteLog(string message)
        {
            _logger.Info(message);
        }
        public static void WriteLog(Exception exception)
        {
            _logger.Error(exception);

        }
    }
}
