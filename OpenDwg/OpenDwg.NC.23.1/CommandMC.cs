using System.ComponentModel;
using System.Diagnostics;
using Multicad.DatabaseServices;
using static dRz.Test.OpenDwg.ServicesCAD;
using System;



#if NC || NC26
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

/*
    рекомендую этот тест запустить на наборе dwg\4к\2k\1k1\500_\
    
    с mcDocument.Dispose() тест пройдет??

*/




namespace dRz.Test.OpenDwg
{
    public partial class CommandMC
    {
        /// <summary>
        /// открытие файлов в цикле в Мультикаде<br/>
        /// в АК 4к файлов 4минуты<br/>
        /// в нк 4к файлов вылетает или виснет<br/>
        /// </summary>
        [CommandMethod("тдм")]
        [Description("открытие файлов в Multicad")]
        public static void MC()
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

            foreach (string file in files)
            {
                counter++;
                logger.Log($"{counter} Opening {file}");

                //если открыт то не нулл
                McDocument mcDocument = McDocumentsManager.GetDocument(file);
                if (mcDocument == null)
                {
                    mcDocument = McDocumentsManager.OpenDocument(file, false, true);

                    if (mcDocument == null)  //проверка на нулл, если нулл то пропуск и записать в лог, что файл пропущен
                    {
                        errors++;
                        loggerErr.Log($"{errors} NULL >> {file} >>");


                        ed.WriteMessage($"NULL >> {file} >> \n");
                        continue;
                    }

                }

                logger.Log($"\t\tWorking {file}");
                reading++;
                // …

                if (mcDocument.IsHidden)
                {
                    mcDocument.Close();//если не открывали не закрывать
                    mcDocument.Dispose();
                    mcDocument = null;
                }

                logger.Log($"\t\tClosed {file}");

                if (counter % 50 == 0)
                {

                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }


                //  mcDocument.Dispose(); //todo костыль рабочий

            }

            //вернем рабочий документ мало ли
            //  McDocument.WorkingDocument = pOldWD;

            stw.Stop();


            string elapsedTime = stw.Elapsed.ToString();

            logger.Log($"Total {files.Length}, Read {reading}, Err {errors}: time {elapsedTime}", 1);

            ed.WriteMessage($"Multicad: Total {files.Length}, Read {reading}, Err {errors}: time {elapsedTime}");

            //GC.Collect();//todo чистим за собой
            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();

        }
    }
}
