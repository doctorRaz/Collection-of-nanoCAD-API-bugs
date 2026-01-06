using NLog;
using System.Globalization;

namespace ConsoleApp
{
    internal class Program
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


        static void Main(string[] args)
        {



            //string folder = @"c:\Users\dRz\AppData\Roaming\Nanosoft\nanoCAD x64 26.0\";
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            string Ppath = Path.Combine(appData, @"Nanosoft\nanoCAD x64 26.0\");

            int a = 4000;//сколько файлов хотим прогнать
                       
            int b = 43;

            int c = (a + b - 1) / b;


            double ff =(double) a / b;



            int numberRepeats = (int)Math.Ceiling( ff);//повторов в цикле




            //https://nlog-project.org/

            string date = DateTime.Now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = $"{date}_nLog.log" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            //config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;


            Logger.Info($"----------------------");


            Logger.Info($"Info");
            Logger.Trace($"Trace");
            Logger.Debug($"Debug");
            Logger.Warn($"Warn");
            Logger.Fatal($"Fatal");
            Logger.Error($"Error");



            return;
            var prop = new Props();

            prop.TestProps();




            #region Logger


            var runnner = new LoggerRun();

            runnner.Run();

            #endregion

        }
    }


}
