using System.ComponentModel;
using System.Diagnostics;
using Multicad.DatabaseServices;
using static dRz.Test.OpenDwg.ServicesTG;
using System;


#if NC
using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using Teigha.Runtime;
using App = HostMgd.ApplicationServices;

#elif AC
using Autodesk.AutoCAD.Runtime;
using App = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

#endif

namespace dRz.Test.OpenDwg
{
    public partial class CommandMCDI
    {
        /// <summary>
        /// открытие файлов в цикле в Мультикаде диспозим
        /// </summary>
        [CommandMethod("тдмди")]
        [Description("открытие файлов в цикле в Мультикаде")]
        public static void MCDI()
        {
            Document doc = App.Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
            {
                return;
            }

            Editor ed = doc.Editor;

            Stopwatch stw = new Stopwatch();

            string folder = Services.Browser();
            string[] files = Services.GetFilesOfDir(folder, true);

            string sender = CallerName(files.Length);


            Logger logger = new Logger($"{sender}");
            Logger loggerErr = new Logger($"{sender} ERR");

            logger.Log($"Total {files.Length} files");

            ed.WriteMessage($"Multicad: Total {files.Length} files");

            stw.Restart();

            //запомним рабочий документ на всякий
            McDocument pOldWD = McDocument.WorkingDocument;

            int counter = 0;
            int reading = 0;
            int errors = 0;
          //  McDocument mcDocument;//шаманство
            foreach (string file in files)
            {
                counter++;
                logger.Log($"{counter} Opening {file}");

                //если открыт то не нулл
                McDocument mcDocument = McDocumentsManager.GetDocument(file);
                if (mcDocument == null)
                {
                    // открываем файл в скрытом режиме
                   // logger.Log($"\t\tOpen {file}");

                    mcDocument = McDocumentsManager.OpenDocument(file, false, true);
                    
                   // logger.Log($"\t\tOpened {file}");

                    if (mcDocument == null)  //проверка на нулл, если нулл то пропуск и записать в лог, что файл пропущен
                    {
                        errors++;
                        loggerErr.Log($"{errors} NULL >> {file} >>");


                        ed.WriteMessage($"NULL >> {file} >> \n");
                        continue;
                    }

                }

              //  logger.Log($"\t\tWorking {file}");
                reading++;
                // …

                if (mcDocument.IsHidden) mcDocument.Close();//если не открывали не закрывать

                logger.Log($"\t\tClosed {file}");

                mcDocument.Dispose(); //todo костыль

                //mcDocument = null;
              
            }

            //вернем рабочий документ мало ли
            McDocument.WorkingDocument = pOldWD;

            stw.Stop();


            string elapsedTime = stw.Elapsed.ToString();

            logger.Log($"Total {files.Length}, Read {reading}, Err {errors}: time {elapsedTime}", 1);

            ed.WriteMessage($"Multicad: Total {files.Length}, Read {reading}, Err {errors}: time {elapsedTime}");

            //GC.Collect();//todo чистим за собой
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }
    }
}
