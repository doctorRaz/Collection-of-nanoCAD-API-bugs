//! Created by dRz on 09.01.2025 at 14:45
using System.ComponentModel;
using System.Reflection;
using drz.Infrastructure.CAD.MessageService;
using drz.Infrastructure.CAD.Service;
using System.IO;

#if NC
using App = HostMgd.ApplicationServices;
using Win = HostMgd.Windows;
using Cad = HostMgd.ApplicationServices.Application;
using Db = Teigha.DatabaseServices;
using Ed = HostMgd.EditorInput;
using Rtm = Teigha.Runtime;
using Gem = Teigha.Geometry;

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

namespace drz.FileOpenClose
{
    /// <summary> 
    /// Команды
    /// </summary>
    internal class CadCommand : Rtm.IExtensionApplication
    {
        CmdInfo CDI = new CmdInfo(Assembly.GetExecutingAssembly(), true);//эта сборка вывод имен классов
        AsmInfo AI = new AsmInfo(Assembly.GetExecutingAssembly());
        Msg msgService = new Msg();

        #region INIT
        public void Initialize()
        {
            ListCMD();
        }

        public void Terminate() { }

        #endregion

        #region Command

        /// <summary>
        /// Lists the command.
        /// </summary>
        [Rtm.CommandMethod("drz_FileOpenClose_info")]
        [Description("Информация о командах сборки")]
        public void ListCMD()
        {


            string sTitleAttribute = AI.sTitleAttribute;
            string sVersion = AI.sVersionFull;
            string sDateRliz = AI.sDateRelis();


            msgService.MsgConsole(sTitleAttribute + ": v." + sVersion + " от " + sDateRliz);

            if (!string.IsNullOrEmpty(CDI.sCmdInfo))
            {
                msgService.MsgConsole(CDI.sCmdInfo);
            }
            else
            {
                msgService.MsgConsole("Нет зарегистрированных команд");
            }

            if (!string.IsNullOrEmpty(CDI.sDuplInfo))
            {
                //msgService.MsgConsole("_____________________");
                msgService.MsgConsole(CDI.sDuplInfo);
                //msgService.MsgConsole("_____________________");

            }
        }

        /// <summary>
        /// Tests the command.
        /// </summary>
        [Rtm.CommandMethod("drz_FileOpenClose")]
        [Description("Тест баги с падением нано при пакетной обрабоике в гпафическом редакторе")]
        public void test_Bug()
        {
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

            string[] dswgPaths = McUtilWorkFil.GetFilesOfDir(sPath, true);
            if (dswgPaths.Length > 0)
            {
                App.Document docWork;

                foreach (string dswgPath in dswgPaths)
                {
                    docWork = Cad.DocumentManager.Open(dswgPath, true);
                    docWork.CloseAndDiscard();//не сохранять
                }
                //docWork.Dispose();
            }

            msgService.MsgConsole(AI.sTitleAttribute + "\nОбработано" + " " + dswgPaths.Length.ToString() + " файлов");
        }

        class McUtilWorkFil
        {


            /// <summary>Получить список путей фалов в директории</summary>
            /// <param name="sPath">Директория с файлами</param>
            /// <param name="WithSubfolders">Учитывать поддиректории</param>
            /// <param name="sSerchPatern">Маска поиска</param>
            /// <returns>Пути к файлам</returns>
            internal static string[] GetFilesOfDir(string sPath, bool WithSubfolders, string sSerchPatern = "*.dwg")
            {
                try
                {
                    return Directory.GetFiles(sPath,
                                                sSerchPatern,
                                                (WithSubfolders
                                                ? SearchOption.AllDirectories
                                                : SearchOption.TopDirectoryOnly));
                }
                catch (System.Exception ex)
                {
#if NC || AC
                    Msg msgService = new Msg();
                    msgService.MsgConsole("\n" + ex.Message);
#endif
                    return new string[0];
                }
            }
        }
        #endregion
    }
}
