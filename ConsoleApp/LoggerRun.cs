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

        internal void Run()
        {

            Logger logger = new Logger(0);


            int count = 10;
            logger.Log($"Total {count.ToString()} files");

            int i = 0;
            for (; i < count; i++)
            {
                logger.Log($"{i}\t\tWorking c:\\dddd\\dddd\\{i}");
            }

            logger.Log($"Teigha\tfiles {count}: time 10:10:100", 1);
          

            Console.WriteLine($"The END");
            Console.ReadKey();

        }
    }
}
