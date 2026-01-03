using NLog;
using NLog.Config;
using System;

namespace ConsoleApp
{

    public class Class1
    {

        private static readonly Logger log = NLog.LogManager.GetCurrentClassLogger();

        internal void test1()
        {
         Program.ConfigureNLog();

            log.Trace($"Trace");
            log.Info($"Info");
            log.Debug($"Debug");
            log.Warn($"Warn");
            log.Error($"Error");
            log.Fatal($"Fatal");

        }



    }
}