//! Created by dRz on 09.01.2025 at 14:45
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using drz.Infrastructure.CAD.Services;
using Teigha.DatabaseServices;



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

namespace drz.NC.NET
{
    /// <summary> 
    /// Команды
    /// </summary>
    internal partial class CadCommand
    {
        //https://adn-cis.org/forum/index.php?topic=9374.msg39381#msg39381

        /// <summary>
        /// Тест записи чтения пользовательских свойств документа
        /// </summary>
        [Rtm.CommandMethod("ttd")]
        [Rtm.CommandMethod("drz_DocProp")]
        [Description("Тест записи чтения пользовательских свойств документа")]
        public void DocProp()
        {
            AsmInfo AI = new AsmInfo(Assembly.GetExecutingAssembly());

            string tempDirectory = Path.GetTempPath();

            string filName = Path.Combine(tempDirectory, AI.sAsmFileNameWithoutExtension + ".dwg");


            Dictionary<string, string> customProperties = new Dictionary<string, string>()
                 {
                     {"prop10", "val1"},
                     {"prop2", "val2"},
                     {"prop3", "val3"},
                     {"prop4", "val3"},
                 };

            EditorDocProp editorDocProp = new EditorDocProp();

            editorDocProp.CustomProperties=customProperties;

            customProperties = new Dictionary<string, string>()
                 {
                     {"prop3", "val3dd"},
                     {"prop2", "val2dd"},
                     {"prop1", "val1ddd"},
                 };

            editorDocProp.CustomProperties=customProperties;

            var prop = editorDocProp.CustomProperties;

            return;
                        //пишем
            SetDwgCustomProp(customProperties);


            SetDwgCustomProp(customProperties);

            //сохранили закрыли
            SaveCloseDwg(filName);

            //открыли прочитали
            ReadDwgCustomPropCommand(filName);

        }

        /// <summary>
        /// Пишем пользовательские свойства в документ
        /// </summary>
        void SetDwgCustomProp(Dictionary<string, string> customProperties)
        {
            App.Document doc = Cad.DocumentManager.MdiActiveDocument;

            Database db = doc.Database;
            foreach (var item in customProperties)
            {
                db.SetDrawingProperty(item.Key, item.Value);
            }


            ////dynamic comDoc = doc.AcadDocument;
            //nanoCAD.Document comDoc = doc.AcadDocument as nanoCAD.Document;



            //// Нет контроля на предмет повтора ключа / наличия аналогичных свойств
            //for (int index = 0; index < customProperties.Count; index++)
            //{


            //    var pair = customProperties.ElementAt(index);

            //    //comDoc.SummaryInfo.GetCustomByKey(pair.Key, out val);

            //    comDoc.SummaryInfo.AddCustomInfo(pair.Key, pair.Value);
            //    comDoc.SummaryInfo.SetCustomByKey(pair.Key, pair.Value);
            //}
        }

        Dictionary<string, string> ReadDwgCustomPropCommand(string filName)
        {

            App.Document doc = Cad.DocumentManager.Open(filName);

            nanoCAD.Document comDoc = doc.AcadDocument as nanoCAD.Document;
            //dynamic comDoc = doc.AcadDocument;

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

            return customProperties;
        }

        //bool IsKeyExist(this)
        //{
        //    return true;
        //}

        /// <summary>
        /// Сохраняем закрываем, но это не обязательно, можно и руками
        /// </summary>
        /// <param name="filName"></param>
        void SaveCloseDwg(string filName)
        {
            App.Document doc = Cad.DocumentManager.MdiActiveDocument;

            doc.CloseAndSave(filName);

            msgService.MsgConsole("Записали и закрыли");

        }



        Msg msgService = new Msg();

    }

    

}
