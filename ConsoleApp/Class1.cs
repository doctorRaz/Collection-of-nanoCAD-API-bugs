using NLog;
using System.Globalization;

namespace ConsoleApp
{

    public partial class Class1
    {

        //private static readonly Logger log = NLog.LogManager.GetCurrentClassLogger();

        internal void test1()
        {
            //Program.ConfigureNLog();

            test2();

            //GlobalDiagnosticsContext.Set("logTimestamp", DateTime.Now.ToString("yyyyMMdd-HH_mm_ss", CultureInfo.InvariantCulture));

            for (int i = 1; i <= Program.count; i++)
            {
                log.Trace($"************  {i} *************");

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