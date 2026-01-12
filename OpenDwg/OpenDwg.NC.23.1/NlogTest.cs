#if DEBUG


using System.ComponentModel;
using System.Diagnostics;
using static dRz.Test.OpenDwg.ServicesCAD;
using NLog;
using System;
using System.Globalization;
using System.IO;
using NLog.Config;







#if NC || NC26
using Teigha.DatabaseServices;
using Teigha.Runtime;
using App = HostMgd.ApplicationServices;
using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using Db = Teigha.DatabaseServices;

#elif AC
using Db = Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.Runtime;
using App = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

#endif


namespace dRz.Test.OpenDwg
{
    public class NlogTest
    {
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();

        static int count = 10;

        /// <summary>
        /// проверка работы лога
        /// </summary>
        [CommandMethod("лог")]
        [Description("проверка работы лога")]
        public static void Log()
        {
            //ConfigureNLog

            string fil = @"d:\@Developers\Programmers\!NET\!nanoCAD-API-bugs\Collection-of-nanoCAD-API-bugs\bin\Debug\NLog.dll";


            LogBootstrap.Init();


            //deb
            NLog.Common.InternalLogger.LogLevel = LogLevel.Trace;

            NLog.Common.InternalLogger.LogFile = Path.Combine(Path.GetTempPath(), "nlog-internal.log");

            GlobalDiagnosticsContext.Set("appName", ServicesCAD.CallerName(count));

            string logTimestamp = $"{DateTime.Now.ToString("yyyyMMdd-HH_mm_ss", CultureInfo.InvariantCulture)}_";

            GlobalDiagnosticsContext.Set("logTimestamp", logTimestamp);

            var config = LogManager.Configuration;

            log.Info("Performance metrics: " +
        "Memory: {MemoryUsage}MB, " +
        /*   "CPU: {CpuUsage}%, " +*/
        "Threads: {ThreadCount}, " +
        "Handles: {HandleCount}",
        Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024,
        /*GetCpuUsage(),*/
        Process.GetCurrentProcess().Threads.Count,
        Process.GetCurrentProcess().HandleCount);


            try
            {
                log.ForInfoEvent()
                   .Message("Начало работы")
                   .Property("userId", "wwweew")
                   .Property("property1", 123)
                   .Log();


                log.Trace($"************  1 *************");
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

                log.Error(new System.Exception(), "This is an error message");




                int e = 0;

                int ii = 10 / e;
            }
            catch (System.Exception ex)
            {
                log.ForErrorEvent()
                   .Exception(ex)
                   .Property("userId", 50000)
                   .Property("property1", 123)
                   .Log();

                log.Info("Продолжение работы после ошибки");

                log.Error(ex);

            }
            finally
            {
                var MemoryUsage = Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024;
                var ThreadCount = Process.GetCurrentProcess().Threads.Count;


                log.ForInfoEvent()
                                   .Message("Performance metrics")
                                   .Property("userId", $"Memory: {MemoryUsage}MB")
                                   .Property("property1", $"Threads: {ThreadCount}")
                                   .Log();

                LogManager.Shutdown();
            }





        }

    }

    public static class LogBootstrap
    {
        public static void Init()
        {
            var dllDir = Path.GetDirectoryName(
                typeof(LogBootstrap).Assembly.Location);

            var configPath = Path.Combine(dllDir, "nlog.config");

            LogManager.Configuration = new XmlLoggingConfiguration(configPath);
        }
    }
}


#endif