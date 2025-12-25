using System.ComponentModel;
using System.Diagnostics;
using Multicad.DatabaseServices;

#if NC
using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using Teigha.Runtime;
using App = HostMgd.ApplicationServices;
using cad = HostMgd.ApplicationServices.Application;

#elif AC
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.Runtime;
using App = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using cad = Autodesk.AutoCAD.ApplicationServices.Application;// ApplicationServices.Application;

#endif

namespace dRz.Test.OpenDwg
{
    public partial class CommandMC
    {
        /// <summary>
        /// открытие файлов в цикле в Мультикаде
        /// </summary>
        [CommandMethod("тдм")]
        [Description("открытие файлов в цикле в Мультикаде")]
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

            string sender = Services.CallerName(files.Length);

            Logger logger = new Logger($"{sender}");
            Logger loggerErr = new Logger($"{sender} ERR");

            logger.Log($"\tTotal {files.Length} files");

            ed.WriteMessage($"Multicad: Total {files.Length} files");

            stw.Restart();

            //запомним рабочий документ на всякий
            McDocument pOldWD = McDocument.WorkingDocument;

            int counter = 0;
            int total = 0;
            int totalErr = 0;
            foreach (string file in files)
            {
                counter++;
                logger.Log($"{counter} Opening {file}");

                //если открыт то не нулл
                McDocument mcDocument = McDocumentsManager.GetDocument(file);
                if (mcDocument == null)
                {
                    // открываем файл в скрытом режиме
                    mcDocument = McDocumentsManager.OpenDocument(file, false, true);
                    if (mcDocument == null)  //проверка на нулл, если нулл то пропуск и записать в лог, что файл пропущен
                    {
                        totalErr++;
                        loggerErr.Log($"\n{totalErr} NULL >> {file} >> \n");


                        ed.WriteMessage($"\n NULL >> {file} >> \n");
                        continue;
                    }

                }

                logger.Log($"\t\tWorking {file}");
                total++;
                // …

                if (mcDocument.IsHidden) mcDocument.Close();//если не открывали не закрывать

                logger.Log($"\t\tClosed {file}");
            }

            //вернем рабочий документ мало ли
            McDocument.WorkingDocument = pOldWD;

            stw.Stop();

            string elapsed = stw.Elapsed.ToString();

            logger.Log($"Multicad\tfiles {total}, err {totalErr}: time {elapsed}");

            ed.WriteMessage($"Multicad\tfiles {total}: time {elapsed}");
        }
    }
}
