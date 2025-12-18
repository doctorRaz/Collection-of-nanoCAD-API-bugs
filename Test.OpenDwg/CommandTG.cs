using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using System.ComponentModel;
using Teigha.Runtime;
using App = HostMgd.ApplicationServices;
using static dRz.Test.OpenDwg.ServicesTG;
using Teigha.DatabaseServices;
using Db = Teigha.DatabaseServices;
using dRz.SpecSPDS.Core.Services;
using System.Diagnostics;
using cad = HostMgd.ApplicationServices.Application;

namespace dRz.Test.OpenDwg
{
    public partial class CommandTG
    {

        /// <summary>
        /// открытие файлов в цикле в Тайге
        /// </summary>
        [CommandMethod("тдт")]
        [Description("открытие файлов в цикле в Тайге")]
        public static void OpenTG()
        {
            Document doc = App.Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
            {
                return;
            }

            Editor ed = doc.Editor;

            Stopwatch stw = new Stopwatch();
            Version version = cad.Version;

            string sender = $"{version.Major.ToString()}.{version.Minor.ToString()}_{nameof(OpenTG)}";

            Logger logger = new Logger(sender);

            string folder = Services.Browser();

            string[] files = Services.GetFilesOfDir(folder, true);


            logger.Log($"\tTotal {files.Length} files");

            ed.WriteMessage($"\tTotal {files.Length} files");

            stw.Start();

            int counter = 1;
            foreach (string file in files)
            {
                logger.Log($"{counter} Opening {file}");
                counter++;
                try
                {
                    using (Database extDBase = new Database(false, true))
                    {

                        extDBase.ReadDwgFile(file, Db.FileOpenMode.OpenTryForReadShare/*OpenForReadAndAllShare*/, false /*true*/, "", false);

                        using (WorkingDatabaseSwitcher dbSwitcher = new WorkingDatabaseSwitcher(extDBase))
                        {

                            logger.Log($"\t\tWorking {file}");
                            // …
                        }
                    }
                }
                catch (System.Exception ex)
                {

                    logger.Log($"\n{file} error : {ex.Message} >> {ex.StackTrace}\n");

                    ed.WriteMessage($"\n{file} error : {ex.Message} >> {ex.StackTrace}\n");
                }

                logger.Log($"\t\tClosed {file}");
            }

            stw.Stop();

            string elapsed = stw.Elapsed.ToString();

            logger.Log($"\tTotal time {elapsed}");

            ed.WriteMessage($"\tTotal time {elapsed}");
        }


    }
}
