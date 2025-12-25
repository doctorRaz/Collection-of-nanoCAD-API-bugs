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
    partial class InitCmd : IExtensionApplication
    {
        public void Initialize()
        {
            Loader.OpenDwg();
            //throw new NotImplementedException();
        }

        public void Terminate()
        {
            //throw new NotImplementedException();
        }
    }

    class Loader
    {
        internal static void OpenDwg()
        {
            Document doc = App.Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
            {
                return;
            }

            Editor ed = doc.Editor;

            ed.WriteMessage($"\nHello test \"OpenDwg\"");
        }
    }
}