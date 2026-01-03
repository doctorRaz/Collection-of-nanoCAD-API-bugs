using NLog;
using NLog.Config;
using System;

namespace ConsoleApp
{

    public partial class Class1
    {

        private static readonly Logger log = NLog.LogManager.GetCurrentClassLogger();

        internal void test2()
        {
            //Program.ConfigureNLog();

            for (int i = 0; i <= Program.count; i++)
            {
                log.Warn($"************  {Program.count} *************");

                log.Trace($"Trace");
                log.Info($"Info");
                log.Debug($"Debug");
                log.Warn($"Warn");
                log.Error($"Error");
                log.Fatal($"Fatal");

            }
        }



    }
}