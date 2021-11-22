using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spirion.Automation.Framework
{
    public class Logger
    {
        public static void InitLogging(string logfile)
        {
            Log.Logger = new LoggerConfiguration()
          .Enrich.FromLogContext()
          .WriteTo.File(logfile)
          .CreateLogger();

        }

        public static void LogInfo(string message)
        {
            Log.Logger.Information(message);
        }

        public static void LogError(string message)
        {
            Log.Logger.Error(message);
        }
    }
}
