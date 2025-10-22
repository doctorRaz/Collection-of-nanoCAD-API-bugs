//! Created by dRz on 09.01.2025 at 14:45
using System.ComponentModel;
using System.Reflection;

using drz.Infrastructure.CAD.Services;

#if NC
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
        /// Lists the command.
        /// </summary>
        [Rtm.CommandMethod("drz_nanoCADNET_info")]
        [Description("Информация о командах сборки")]
        public static void ListCmd()
        {
            CmdInfo CDI = new CmdInfo(Assembly.GetExecutingAssembly(), true);//эта сборка вывод имен классов
            AsmInfo AI = new AsmInfo(Assembly.GetExecutingAssembly());
            Msg msgService = new Msg();

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
                msgService.MsgConsole(CDI.sDuplInfo);
            }
        }
    }
}
