using NLog;
using NLog.Config;
using NLog.Targets;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ConsoleApp
{
    internal class Program
    {

        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();


        public static void ConfigureNLog([CallerMemberName] string caller = null)
        {
            string date = DateTime.Now.ToString("yyyyMMdd-HH_mm_ss", CultureInfo.InvariantCulture);

            var config = new LoggingConfiguration();

            // Target для отдельного класса
            var fileTarget = new FileTarget
            {
                Name = $"{caller}",
                FileName = $"{date}_{caller}.log",
                //Layout = "${longdate} ${level:uppercase=true} ${message}"
            };

            var fileTargetErr = new FileTarget
            {
                Name = $"{caller}",
                FileName = $"{date}_{caller}_Err.log",
                //Layout = "${longdate} ${level:uppercase=true} ${message}"
            };

            config.AddRule(LogLevel.Trace, LogLevel.Warn, fileTarget);
             config.AddRule(LogLevel.Error, LogLevel.Fatal, fileTargetErr);

            //config.AddTarget(fileTarget);

            //// Правило только для конкретного класса
            //config.AddRuleForOneLevel(LogLevel.Trace, fileTarget, "MyNamespace.MyClass");

            LogManager.Configuration = config;
        }

        static void Main(string[] args)
        {

            ConfigureNLog();

            //https://nlog-project.org/
            /*

            LoggingConfiguration config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            FileTarget logfile = new FileTarget("logfile") { FileName = $"{date}_nLog.log" };
            var logfileErr = new FileTarget("logfile") { FileName = $"{date}_Err_nLog.log" };
            ColoredConsoleTarget logconsole = new NLog.Targets.ColoredConsoleTarget("logconsole");
            

            // Rules for mapping loggers to targets            
            //config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);
            config.AddRule(LogLevel.Error, LogLevel.Fatal, logfileErr);

            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logconsole);

            // Apply config           
            LogManager.Configuration = config;
            */

            log.Info("This is a message from {User}", "Mickey Donovan");

            var msg = new LogEventInfo(LogLevel.Info, "", "This is a message");
            msg.Properties.Add("User", "Ray Donovan");
            log.Info(msg);

            log.Info(string.Format("This is a message from {0}", "Mickey Donovan"));


            log.Trace($"Trace");
            log.Debug($"Debug");
            log.Info($"Info");
            log.Warn($"Warn");
            log.Error($"Error");
            log.Fatal($"Fatal");

            log.Error(new Exception(), "This is an error message");

            Class1 class1 = new Class1();
            class1.test1();


        }
    }


}
