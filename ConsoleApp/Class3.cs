namespace ConsoleApp
{

    public partial class Class1
    {

        //private static readonly Logger log = NLog.LogManager.GetCurrentClassLogger();

        internal void test3()
        {
            //Program.ConfigureNLog();


            for (int i = 1; i <= Program.count; i++)
            {
                log.Warn($"************  {i} *************");

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