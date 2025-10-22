//! Created by dRz on 09.01.2025 at 14:45
using drz.Infrastructure.CAD.Services;
using System.IO;

#if NC

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

namespace drz.Utilities
{
    /// <summary> 
    /// Команды
    /// </summary>
    internal class UtilWorkFil
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
}
