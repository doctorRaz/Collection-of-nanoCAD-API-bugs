using dRz.Test.OpenDwg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{


    internal class LoggerRun
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        internal void Run()
        {

            Logger logger = new Logger();


            int count = 10 ;
         //   logger.Log($"Total {count.ToString()} files");
            Logger.Info($"Total {count.ToString()} files");

            int i = 0;
            for (; i < count; i++)
            {
                //  logger.Log($"{i}\t\tWorking c:\\dddd\\dddd\\{i}");
                Logger.Info($"{i}\t\tWorking c:\\dddd\\dddd\\{i}");
            }

            Logger.Info($"Teigha\tfiles {count}: time 10:10:100", 1);
            //logger.Log($"Teigha\tfiles {count}: time 10:10:100", 1);


            Console.WriteLine($"The END");
            Console.ReadKey();

        }
    }
}
