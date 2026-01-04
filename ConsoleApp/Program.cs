using NLog;
using NLog.Config;
using NLog.Targets;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ConsoleApp
{
    internal class Program
    {
        internal static int count = 3;

        private static readonly Logger log = LogManager.GetCurrentClassLogger();


        public static void ConfigureNLog([CallerMemberName] string caller = null)
        {
            string date = DateTime.Now.ToString("yyyyMMdd-HH_mm_ss", CultureInfo.InvariantCulture);

            string appName = Services.CallerName(count);    

            string name = "${callsite:className=false:methodName=true:includeSourcePath=false}";

            string fileName = $"${{basedir}}/logs/{date}_${{callsite:className=false:methodName=true:includeSourcePath=false}}_{appName}";

            LoggingConfiguration config = new LoggingConfiguration();

            // Target для отдельного класса
            FileTarget fileTarget = new FileTarget
            {
                //Name = $"{caller}",
                Name =name,// "${callsite:className=false:methodName=true:includeSourcePath=false}",
                //FileName = "${date:format=yyyyMMdd-HH_mm_ss}_${callsite:className=false:methodName=true:includeSourcePath=false}.log",
                FileName =$"{fileName}.log",// $"${{basedir}}/logs/{date}_${{callsite:className=false:methodName=true:includeSourcePath=false}}.log",
                Layout = "${longdate} | " +
                         "${level:uppercase=true} | " +
                         "${callsite:className=true:methodName=true:includeSourcePath=false} | " +
                         "Line:${callsite-linenumber} | " +
                         "${message} | " +
                         "${exception:format=ToString}"
            };

            FileTarget fileTargetErr = new FileTarget
            {
                //Name = $"{caller}",
                Name =name,// "${callsite:className=false:methodName=true:includeSourcePath=false}_Err",
                //FileName = "${date:format=yyyyMMdd-HH_mm_ss}_${callsite:className=false:methodName=true:includeSourcePath=false}_Err.log",
                FileName =$"{fileName}_Err.log",// $"${{basedir}}/logs/${date}_${{callsite:className=false:methodName=true:includeSourcePath=false}}_Err.log",
                Layout = "${longdate} | ${level:uppercase=true} | " +
                         "${callsite:methodName=true} | " +
                         "User[${windows-identity}] | " +
                         "Session:${aspnet-sessionid} | " +
                         "Request:${aspnet-request-url} | " +
                         "${message} | " +
                         "${when:when=length('${exception}')>0:Inner=${newline}StackTrace: ${exception:format=StackTrace}}"
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


            var sw = new Stopwatch();

            sw.Start();

            for (int i = 1; i <= count; i++)
            {
                log.Warn($"************  {i} *************");
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
            }

            Class1 class1 = new Class1();

            class1.test1();

            class1.test2();

            class1.test3();

            sw.Stop();


            var elapsed = sw.Elapsed;


            Console.WriteLine(elapsed.ToString());

            Console.ReadKey();
        }
    }


}
