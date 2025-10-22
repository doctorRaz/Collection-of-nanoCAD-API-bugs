//! Created by dRz on 09.01.2025 at 14:45
using System.ComponentModel;
using System.Reflection;

using drz.Infrastructure.CAD.Services;
using drz.Utilities;

#if NC
using App = HostMgd.ApplicationServices;
using Cad = HostMgd.ApplicationServices.Application;
using Rtm = Teigha.Runtime;

#elif AC
using Autodesk.AutoCAD.Windows;
using App = Autodesk.AutoCAD.ApplicationServices;
using Cad = Autodesk.AutoCAD.ApplicationServices.Application;
using Db = Autodesk.AutoCAD.DatabaseServices;
using Ed = Autodesk.AutoCAD.EditorInput;
using Gem = Autodesk.AutoCAD.Geometry;
using Rtm = Autodesk.AutoCAD.Runtime;
#endif
// Reserved template parameters
// itemname - CadCommand
// machinename - WIN-CGR
// projectname	 - Test CMD INFO
// registeredorganization - 
// rootnamespace - $rootnamespace$
// defaultnamespace - $defaultnamespace$
// safeitemname - CadCommand
// safeitemrootname - CadCommand
// safeprojectname - Test_CMD_INFO
// targetframeworkversion - 4.7.2
// time - 21.12.2024 21:14:12"
// specifiedsolutionname - nanoCADCommandsReflection
// userdomain - WIN-CGR
// username - dRz"
// webnamespace - $webnamespace$
// year - 2024



//https://learn.microsoft.com/en-us/visualstudio/ide/template-parameters?view=vs-2022

namespace drz.nanoCAD.NET
{
    /// <summary> 
    /// Команды
    /// </summary>
    internal partial class CadCommand
    {
        /// <summary>
        /// Tests the command.
        /// </summary>
        [Rtm.CommandMethod("drz_FileOpenClose")]
        [Description("Тест баги с падением nanoCAD при пакетной обработке в графическом редакторе")]
        public void FileOpenCloseCmd()
        {

            AsmInfo AI = new AsmInfo(Assembly.GetExecutingAssembly());
            Msg msgService = new Msg();

            FolderBrowserDialog FDB = new FolderBrowserDialog();
            FDB.Description = "Выберите каталог";

            string sPath = "";
            if (FDB.ShowDialog() == DialogResult.OK)
            {
                sPath = FDB.SelectedPath;
            }
            else
            {
                Msg msg = new Msg();
                msg.MsgConsole("Выбор отменен");
                return;
            }

            string[] dswgPaths = UtilWorkFil.GetFilesOfDir(sPath, true);
            if (dswgPaths.Length > 0)
            {
                App.Document docWork;

                foreach (string dswgPath in dswgPaths)
                {
                    docWork = Cad.DocumentManager.Open(dswgPath, true);
                    docWork.CloseAndDiscard();//не сохранять
                }
            }

            msgService.MsgConsole(AI.sTitleAttribute + "\nОбработано" + " " + dswgPaths.Length.ToString() + " файлов");
        }

    }
}
