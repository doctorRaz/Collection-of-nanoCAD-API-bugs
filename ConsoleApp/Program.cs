using NLog;
using System.Globalization;

namespace ConsoleApp
{
    internal class Program
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


        static void Main(string[] args)
        {
            //https://nlog-project.org/

            string date = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = $"{date}_nLog.log" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;

            
            var prop = new Props();

            prop.TestProps();


        

            #region Logger


          var runnner = new LoggerRun();

          runnner.Run();

            #endregion

        }
    }


}
