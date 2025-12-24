using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using Teigha.Runtime;
using App = HostMgd.ApplicationServices;

namespace ddRz.Test.OpenDwg
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

            ed.WriteMessage($"Hello OpenDwg");
        }
    }
}