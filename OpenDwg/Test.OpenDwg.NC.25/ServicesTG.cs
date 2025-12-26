using System;
using System.Runtime.CompilerServices;


#if NC
using cad = HostMgd.ApplicationServices.Application;
using Teigha.DatabaseServices;

#elif AC
using Autodesk.AutoCAD.DatabaseServices;
using cad = Autodesk.AutoCAD.ApplicationServices.Application;// ApplicationServices.Application;

#endif

namespace dRz.Test.OpenDwg
{
    internal class ServicesTG
    {
  

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

    }
}
