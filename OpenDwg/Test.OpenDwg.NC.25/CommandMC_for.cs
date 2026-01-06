using System.ComponentModel;
using System.Diagnostics;
using Multicad.DatabaseServices;
using static dRz.Test.OpenDwg.ServicesTG;
using System;
using System.IO;




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
    public partial class CommandMC_for
    {
        /// <summary>
        /// открытие файлов из c:\Users\User\AppData\Roaming\Nanosoft\nanoCAD x64 26.0\ в  цикле 10000 итераций в Мультикаде <br/>
        /// нк Total 10019, Read 10019, Err 0: time 00:12:43.3438982<br/>
        /// ак Total 10019, Read 10019, Err 0: time 00:01:20.3801433<br/>
        /// почти в 12 раз!!!!
        /// </summary>
        [CommandMethod("тдмц")]
        [Description("открытие файла в цикле в Мультикаде")]
        public static void MCS()
        {
            Document doc = App.Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
            {
                return;
            }

            Editor ed = doc.Editor;

            Stopwatch stw = new Stopwatch();

            int manyFiles = 10000;//сколько файлов хотим прогнать

            //string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //string folder = Path.Combine(appData, @"Nanosoft\nanoCAD x64 26.0\");

            string folder = Services.Browser();

            string[] files = Services.GetFilesOfDir(folder, true, "*.dw*");//работаем все файлы которые умеем

            int numberRepeats = (manyFiles + files.Length - 1) / files.Length;//повторов в цикле

            int totalFiles = numberRepeats * files.Length;//фактически сколько прогоним с учетом повторов

            string sender = CallerName(numberRepeats * files.Length);


            Logger logger = new Logger($"{sender}");
            Logger loggerErr = new Logger($"{sender} ERR");

            logger.Log($"Total {totalFiles} files");

            ed.WriteMessage($"Multicad: Total {totalFiles} files");

            stw.Restart();

            //запомним рабочий документ на всякий
            McDocument pOldWD = McDocument.WorkingDocument;

            int counter = 0;
            int reading = 0;
            int errors = 0;
            //McDocument mcDocument;

            //пока не упадет или не повиснет
            for (int i = 0; i < numberRepeats; i++)
            {
                McDocument mcDocument;
                foreach (string file in files)
                {

                    counter++;
                    logger.Log($"{counter} Opening {file}");

                    //если открыт то не нулл
                    mcDocument = McDocumentsManager.GetDocument(file);

                    if (mcDocument == null)
                    {

                        // открываем файл в скрытом режиме
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

                    if (mcDocument.IsHidden) mcDocument.Close();//если не открывали не закрывать

                    logger.Log($"\t\tClosed {file}");
                }
            }

            //вернем рабочий документ мало ли
            McDocument.WorkingDocument = pOldWD;

            stw.Stop();

            string elapsedTime = stw.Elapsed.ToString();

            logger.Log($"Total {totalFiles}, Read {reading}, Err {errors}: time {elapsedTime}", 1);

            ed.WriteMessage($"Multicad: Total {totalFiles}, Read {reading}, Err {errors}: time {elapsedTime}");

        }
    }
}
