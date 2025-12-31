using System;
using System.Runtime.CompilerServices;
using System.IO;

#if NC
using cad = HostMgd.ApplicationServices.Application;
using Teigha.DatabaseServices;
using HostMgd.EditorInput;
using HostMgd.ApplicationServices;

#elif AC
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using cad = Autodesk.AutoCAD.ApplicationServices.Application;// ApplicationServices.Application;

#endif

namespace dRz.Test.OpenDwg
{
    internal class ServicesTG
    {

        internal static string GetFileOpenDocProperties(string title = "Выберите файл для открытия")
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            PromptOpenFileOptions options = new PromptOpenFileOptions(title);
            options.Filter = "Файлы dwg (*.dwg)|*.dwg|Файлы dfx (*.dfx)|*.dfx";
            options.FilterIndex = 0;

            options.PreferCommandLine = IsCmdActive();
            options.InitialFileName =
                Path.GetFileNameWithoutExtension(Application.GetSystemVariable("dwgname").ToString());

            PromptFileNameResult fileNameResult = ed.GetFileNameForOpen(options);
            if (fileNameResult.Status == PromptStatus.OK)
            {
                return fileNameResult.StringResult;
            }

            return string.Empty;
        }



        internal static string CallerName(int CountFiles, [CallerMemberName] string caller = null)
        {
            Version version = cad.Version;

            string appProductName = System.Windows.Forms.Application.ProductName;

            return $"{caller}_{appProductName}_{version.Major.ToString()}.{version.Minor.ToString()}_{CountFiles.ToString()}";

        }

        public sealed class WorkingDatabaseSwitcher : IDisposable
        {
            /// <summary> Переключение рабочей базы файла. Обязательно использование через <code>using</code> </summary>
            /// <param name="db">Устанавливаемая рабочей база чертежа</param>
            public WorkingDatabaseSwitcher(Database db)
            {
                _prevDb = HostApplicationServices.WorkingDatabase;
                _needToSwitch = !db.Equals(_prevDb);
                if (_needToSwitch)
                {
                    HostApplicationServices.WorkingDatabase = db;
                }
            }

            public void Dispose()
            {
                if (_needToSwitch)
                {
                    HostApplicationServices.WorkingDatabase = _prevDb;
                    _prevDb = null;
                }
            }

            private Database _prevDb = null;
            private bool _needToSwitch;

        }


        static bool IsCmdActive()
        {
            object cmdactive = Application.GetSystemVariable("CMDACTIVE");
            if ((int)cmdactive == 0)
            {
                //window
                return false;
            }
            else
            {
                //cmd
                return true;
            }
        }
    }
}
