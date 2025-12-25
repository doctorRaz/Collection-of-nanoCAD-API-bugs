using System.ComponentModel;
using System.Diagnostics;
using static dRz.Test.OpenDwg.ServicesTG;

#if NC
using Teigha.DatabaseServices;
using Teigha.Runtime;
using App = HostMgd.ApplicationServices;
using cad = HostMgd.ApplicationServices.Application;
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
using cad = Autodesk.AutoCAD.ApplicationServices.Application;// ApplicationServices.Application;

#endif


namespace dRz.Test.OpenDwg
{
    public partial class CommandTG
    {

        /// <summary>
        /// открытие файлов в цикле в Тайге
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

            string sender = Services.CallerName(files.Length);

            Logger logger = new Logger($"{sender}");

            Logger loggerErr = new Logger($"{sender} ERR");

            logger.Log($"\tTotal {files.Length} files");

            ed.WriteMessage($"Teigha: Total {files.Length} files");

            stw.Restart();

            int counter = 0;
            int total = 0;
            int totalErr = 0;

            foreach (string file in files)
            {
                counter++;
                logger.Log($"{counter} Opening {file}");
                try
                {
                    using (Database extDBase = new Database(false, true))
                    {
#if NC
                        extDBase.ReadDwgFile(file, Db.FileOpenMode.OpenTryForReadShare/*OpenForReadAndAllShare*/, false /*true*/, "", false);
#elif AC
                        extDBase.ReadDwgFile(file, Db.FileOpenMode.OpenForReadAndAllShare, false, "");
#endif
                        using (WorkingDatabaseSwitcher dbSwitcher = new WorkingDatabaseSwitcher(extDBase))
                        {

                            logger.Log($"\t\tWorking {file}");
                            total++;
                            // …
                        }
                    }
                }
                catch (System.Exception ex)
                {

                    totalErr++;
                    loggerErr.Log($"\n{totalErr} {file} error : {ex.Message} >> {ex.StackTrace}\n");


                    ed.WriteMessage($"\n{file} error : {ex.Message} >> {ex.StackTrace}\n");
                }

                logger.Log($"\t\tClosed {file}");
            }

            stw.Stop();

            string elapsed = stw.Elapsed.ToString();

            logger.Log($"Teigha\tfiles {total}: time {elapsed}");

            ed.WriteMessage($"Teigha\tfiles {total}, errr {totalErr}: time {elapsed}");
        }


    }
}
