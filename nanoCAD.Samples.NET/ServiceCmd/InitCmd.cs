//! Created by dRz on 09.01.2025 at 14:45

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

namespace drz.NC.NET
{
    /// <summary> 
    /// Команды
    /// </summary>
    internal class InitCmd : Rtm.IExtensionApplication
    {
        public void Initialize()
        {
            CadCommand.ListCmd();
        }

        public void Terminate() { }

    }
}
