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
        /// Тест записи всех свойств документа  через класс
        /// </summary>
        [Rtm.CommandMethod("tps")]
        [Rtm.CommandMethod("drz_SetProp")]
        [Description("Тест чтения основных свойств документа")]
        public void SetProp()
        {
            Dictionary<string, string> constProperties = new Dictionary<string, string>()
                  {
                         {constProp.Title.ToString(),"Title"},
                         {constProp.Subject.ToString(),"Subject"},
                         {constProp.RevisionNumber.ToString(),"RevisionNumber"},
                         {constProp.LastSavedBy.ToString(),"LastSavedBy"},
                         {constProp.Keywords.ToString(),"Keywords"},
                         {constProp.HyperlinkBase.ToString(),"HyperlinkBase"},
                         {constProp.Comments.ToString(),"Comments"},
                         {constProp.Author.ToString(),"Author"},
                  };

            Dictionary<string, string> customProperties = new Dictionary<string, string>()
                  {
                      {"prop10<>/\":;?*|,='Ok<>/\":;?*|,='Ok", "val1"},
                      {" ", " spa ce "},
                      {"", "  empty  "},
                      {"pr\nop3", "va\nl3"},
                      {"prop4", "val3"},
                      {"prop40", " x  "},
                  };

            Dictionary<string, Dictionary<string, string>> prop = new Dictionary<string, Dictionary<string, string>>
            {
                { propProp.Const.ToString(), constProperties },
                { propProp.Custom.ToString(), customProperties }
            };

            EditorDocProp editorDocProp = new EditorDocProp();

            editorDocProp.Props = prop;
        }


        /// <summary>
        /// Тест чтения всех свойств документа  через класс
        /// </summary>
        [Rtm.CommandMethod("tpr")]
        [Rtm.CommandMethod("drz_GetProp")]
        [Description("Тест чтения основных свойств документа")]
        public void GetProp()
        {
            EditorDocProp editorDocProp = new EditorDocProp();
            Dictionary<string, Dictionary<string, string>> prop = editorDocProp.Props;

            //foreach (var sinf in prop)
            //{
            //    var ii=sinf.Value;

            //}
        }

        /// <summary>
        /// Тест записи основных свойств документа  через класс
        /// </summary>
        [Rtm.CommandMethod("tcs")]
        [Rtm.CommandMethod("drz_SetConstProp")]
        [Description("Тест записи основных свойств документа")]
        public void SetConstProp()
        {
            Dictionary<string, string> customProperties = new Dictionary<string, string>()
                 {
                        {constProp.Title.ToString(),"Title"},
                        {constProp.Subject.ToString(),"Subject"},
                        {constProp.RevisionNumber.ToString(),"RevisionNumber"},
                        {constProp.LastSavedBy.ToString(),"LastSavedBy"},
                        {constProp.Keywords.ToString(),"Keywords"},
                        {constProp.HyperlinkBase.ToString(),"HyperlinkBase"},
                        {constProp.Comments.ToString(),"Comments"},
                        {constProp.Author.ToString(),"Author"},
                 };

            EditorDocProp editorDocProp = new EditorDocProp();

            editorDocProp.ConstProperties = customProperties;
        }

        /// <summary>
        /// Тест чтения основных свойств документа  через класс
        /// </summary>
        [Rtm.CommandMethod("tcr")]
        [Rtm.CommandMethod("drz_GetConstProp")]
        [Description("Тест чтения основных свойств документа")]
        public void GetConstProp()
        {
            EditorDocProp editorDocProp = new EditorDocProp();

            //читаем
            Dictionary<string, string> prop = editorDocProp.ConstProperties;

            if (prop is null || prop.Count < 1)
            {
                msgService.MsgConsole("Err");
            }
            else
            {
                foreach (var item in prop)
                {
                    msgService.MsgConsole($"{item.Key} -> {item.Value}");
                }
                //msgService.MsgConsole("Ok");
            }

        }
        /// <summary>
        /// Тест чтения пользовательских свойств документа  через класс
        /// </summary>
        [Rtm.CommandMethod("ttr")]
        [Rtm.CommandMethod("drz_GetCustomcProp")]
        [Description("Тест чтения пользовательских свойств документа")]
        public void GetCustomcProp()
        {
            EditorDocProp editorDocProp = new EditorDocProp();

            //читаем
            Dictionary<string, string> prop = editorDocProp.CustomProperties;

            if (prop is null || prop.Count < 1)
            {
                msgService.MsgConsole("Err");
            }
            else
            {
                foreach (var item in prop)
                {
                    msgService.MsgConsole($"{item.Key} -> {item.Value}");
                }
                //msgService.MsgConsole("Ok");
            }
        }

        /// <summary>
        /// Тест записи пользовательских свойств документа через класс
        /// </summary>
        [Rtm.CommandMethod("tts")]
        [Rtm.CommandMethod("drz_SetCustomProp")]
        [Description("Тест записи пользовательских свойств документа")]
        public void SetCustomProp()
        {
            Dictionary<string, string> customProperties = new Dictionary<string, string>()
                 {
                     {"prop10<>/\":;?*|,='Ok<>/\":;?*|,='Ok", "val1"},
                     {" ", "space"},
                     {"", "empty"},
                     {"prop3", "val3"},
                     {"prop4", "val3"},
                     {"prop40", ""},
                 };

            EditorDocProp editorDocProp = new EditorDocProp();

            //пишем
            editorDocProp.CustomProperties = customProperties;

            //сохранили закрыли
            //SaveCloseDwg();           

        }

        /// <summary>
        /// Сохраняем закрываем, но это не обязательно, можно и руками
        /// </summary>
        /// <param name="filName"></param>
        void SaveCloseDwg(string filName = null)
        {
            if (filName == null)
            {
                AsmInfo AI = new AsmInfo(Assembly.GetExecutingAssembly());

                string tempDirectory = Path.GetTempPath();

                filName = Path.Combine(tempDirectory, AI.sAsmFileNameWithoutExtension + ".dwg");
            }

            App.Document doc = Cad.DocumentManager.MdiActiveDocument;

            doc.CloseAndSave(filName);

            msgService.MsgConsole("Записали и закрыли");

        }

        Msg msgService = new Msg();

    }



}
