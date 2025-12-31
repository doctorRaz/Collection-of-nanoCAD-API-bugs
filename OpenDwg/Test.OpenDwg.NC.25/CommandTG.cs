using System.ComponentModel;
using System.Diagnostics;
using static dRz.Test.OpenDwg.ServicesTG;


#if NC
using Teigha.DatabaseServices;
using Teigha.Runtime;
using App = HostMgd.ApplicationServices;
using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using Db = Teigha.DatabaseServices;

#elif AC
using Db = Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.Runtime;
using App = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

#endif


namespace dRz.Test.OpenDwg
{
    public partial class CommandTG
    {

        /// <summary>
        /// открытие файлов в чисто в Тайге
        /// нк 4к файлов 6 минут
        /// ак 4к файлов 8 минут
        /// хоть тут его уделал
        /// </summary>
        [CommandMethod("тдт")]
        [Description("открытие файлов в цикле в Тайге")]
        public static void TG()
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

            ed.WriteMessage($"Teigha: Total {files.Length} files");

            stw.Restart();

            int counter = 0;
            int reading = 0;
            int errors = 0;

            foreach (string file in files)
            {
                counter++;
                logger.Log($"{counter} Opening {file}");
                try
                {
                    using (Database extDBase = new Database(false, true))
                    {

                        extDBase.ReadDwgFile(file, Db.FileOpenMode.OpenForReadAndAllShare, false, "");

                        using (WorkingDatabaseSwitcher dbSwitcher = new WorkingDatabaseSwitcher(extDBase))
                        {

                            logger.Log($"\t\tWorking {file}");
                            reading++;
                            // …
                        }
                    }
                }
                catch (System.Exception ex)
                {

                    errors++;
                    loggerErr.Log($"{errors} {file} error : {ex.Message} >> {ex.StackTrace}\n");


                    ed.WriteMessage($"\n{file} error : {ex.Message} >> {ex.StackTrace}\n");
                }

                logger.Log($"\t\tClosed {file}");
            }

            stw.Stop();


            string elapsedTime = stw.Elapsed.ToString();

            logger.Log($"Total {files.Length}, Read {reading}, Err {errors}: time {elapsedTime}", 1);

            ed.WriteMessage($"Teigha: Total {files.Length}, Read {reading}, Err {errors}: time {elapsedTime}");

            //GC.Collect();
            //GC.WaitForPendingFinalizers();
            //GC.Collect();
        }



    }
}
