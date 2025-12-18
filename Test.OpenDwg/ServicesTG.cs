using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teigha.DatabaseServices;

namespace dRz.Test.OpenDwg
{
    internal class ServicesTG
    {

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
