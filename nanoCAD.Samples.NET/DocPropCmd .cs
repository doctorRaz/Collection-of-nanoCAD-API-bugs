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
                     {"prop40", ""},
                 };

            EditorDocProp editorDocProp = new EditorDocProp();

            //пишем
            editorDocProp.CustomProperties=customProperties;

            //сохранили закрыли
            SaveCloseDwg(filName);
            
            Cad.DocumentManager.Open(filName);
            //читаем
            Dictionary<string, string> prop = editorDocProp.CustomProperties;

            if(prop is null || prop.Count<1)
            {
                msgService.MsgConsole("Err");
            }
            else
            {
                msgService.MsgConsole("Ok");

            }
        }     

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
