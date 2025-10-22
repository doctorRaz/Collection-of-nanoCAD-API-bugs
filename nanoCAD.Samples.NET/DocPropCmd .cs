//! Created by dRz on 09.01.2025 at 14:45
using System.ComponentModel;
using System.IO;
using System.Reflection;

using drz.Infrastructure.CAD.Services;


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

namespace drz.nanoCAD.NET
{
    /// <summary> 
    /// Команды
    /// </summary>
    internal partial class CadCommand
    {
        //знаю что так лучше не надо делать
        Msg msgService = new Msg();

        /// <summary>
        /// Тест записи чтения пользовательских свойств документа
        /// </summary>
        [Rtm.CommandMethod("drz_DocProp")]
        [Description("Тест записи чтения пользовательских свойств документа")]
        public void DocProp()
        {
            AsmInfo AI = new AsmInfo(Assembly.GetExecutingAssembly());

            string tempDirectory = Path.GetTempPath();

            string filName = Path.Combine(tempDirectory, AI.sAsmFileNameWithoutExtension + ".dwg");

            //пишем
            SetDwgCustomPropCommand();

            //сохранили закрыли
            SaveCloseDwg(filName);

            //открыли прочитали
            ReadDwgCustomPropCommand(filName);

        }

        void SaveCloseDwg(string filName)
        {
            App.Document doc = Cad.DocumentManager.MdiActiveDocument;

            doc.CloseAndSave(filName);

            msgService.MsgConsole("Записали и закрыли");

        }

        void SetDwgCustomPropCommand()
        {
            App.Document doc = Cad.DocumentManager.MdiActiveDocument;

            Dictionary<string, string> customProperties = new Dictionary<string, string>()
            {
                {"prop1", "val1"},
                {"prop2", "val2"},
                {"prop3", "val3"},
            };

            dynamic comDoc = doc.AcadDocument;

            // Нет контроля на предмет повтора ключа / наличия аналогичных свойств
            for (int index = 0; index < customProperties.Count; index++)
            {
                var pair = customProperties.ElementAt(index);
                comDoc.SummaryInfo.AddCustomInfo(pair.Key, pair.Value);
            }
        }

        void ReadDwgCustomPropCommand(string filName)
        {

            App.Document doc = Cad.DocumentManager.Open(filName);

            dynamic comDoc = doc.AcadDocument;

            Dictionary<string, string> customProperties = new Dictionary<string, string>();

            for (int propIndex = 0; propIndex < comDoc.SummaryInfo.NumCustomInfo(); propIndex++)
            {
                comDoc.SummaryInfo.GetCustomByIndex(propIndex, out string key, out string value);
                customProperties.Add(key, value);
            }

            foreach (KeyValuePair<string, string> pair in customProperties)
            {
                msgService.MsgConsole(pair.Key + "\" => \"" + pair.Value + "\"");

            }
        }
    }
}
