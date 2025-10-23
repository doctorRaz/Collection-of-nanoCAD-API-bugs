using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
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

namespace drz.Infrastructure.CAD.Services
{
    internal class EditorDocProp
    {
        public EditorDocProp(Database db = null)
        {
            if (db != null)
            {
                _db = db;
            }
            else
            {
                App.Document doc = Cad.DocumentManager.MdiActiveDocument;
                _db = doc.Database;
            }

            _summaryInfoBuilder = new DatabaseSummaryInfoBuilder(_db.SummaryInfo);

            _customPropTable = _summaryInfoBuilder.CustomPropertyTable;

        }

        /// <summary>
        /// Удаляем пользовательские свойств
        /// </summary>
        /// <param name="removCustomProperties">словарик удаляемых параметров, значения не нужны</param>
        internal void RemoveCustomProps(Dictionary<string, string> removCustomProperties)
        {

            foreach (var prop in removCustomProperties)
            {
                if (_customPropTable.Contains(prop.Key))
                    _customPropTable.Remove(prop.Key);
            }
            _customPropTable = _summaryInfoBuilder.CustomPropertyTable;
        }

        internal void ClearCustomProps()
        {
            _customPropTable.Clear();
            _customPropTable = _summaryInfoBuilder.CustomPropertyTable;

        }

        internal Dictionary<string, string> ConstProperties
        {
            get
            {
                if (_db == null) return null;

                _constProperties = new Dictionary<string, string>
                {
                    {constProp.Title.ToString(), _summaryInfoBuilder.Title },
                    {constProp.Subject.ToString(), _summaryInfoBuilder.Subject },
                    {constProp.RevisionNumber.ToString(), _summaryInfoBuilder.RevisionNumber },
                    {constProp.LastSavedBy.ToString(), _summaryInfoBuilder.LastSavedBy },
                    {constProp.Keywords.ToString(), _summaryInfoBuilder.Keywords },
                    {constProp.HyperlinkBase.ToString(), _summaryInfoBuilder.HyperlinkBase },
                    {constProp.Comments.ToString(), _summaryInfoBuilder.Comments },
                    {constProp.Autor.ToString(), _summaryInfoBuilder.Author },
                };
                return _constProperties;
            }
            set
            {
                //todo проверку на наличие ключа!!!
                _summaryInfoBuilder.Title = value[constProp.Title.ToString()];

                _db.SummaryInfo = _summaryInfoBuilder.ToDatabaseSummaryInfo();
            }

        }

        /// <summary>
        /// Gets or sets the custom properties dwg.
        /// </summary>
        /// <value>
        /// The custom properties.
        /// </value>
        internal Dictionary<string, string> CustomProperties
        {
            get
            {
                if (_db == null) return null;

                _customProperties   /*Dictionary<string, string> customProperties*/ = new Dictionary<string, string>();

                foreach (DictionaryEntry item in _customPropTable)
                {
                    _customProperties.Add(item.Key.ToString(), item.Value.ToString());
                }

                return _customProperties;
            }
            set
            {
                if (_db == null) return;

                foreach (KeyValuePair<string, string> item in value)
                {
                    string key = item.Key;

                    if (string.IsNullOrWhiteSpace(key))//если ключ без имени
                    {
                        key = "_";
                    }

                    //проверка Key на запрещенные символы, менять на _
                    //<>/\":;?*|,='
                    string pattern = @"[\<\>\/\:\\\"";\?\*\|\,\=\']";

                    key = Regex.Replace(key, pattern, "_");

                    key = Regex.Replace(key, @"_+", "_");//несколько _ на один

                    if (_customPropTable.Contains(key))
                    {
                        _customPropTable[key] = item.Value;
                    }
                    else
                    {
                        _customPropTable.Add(key, item.Value);
                    }
                }

                _db.SummaryInfo = _summaryInfoBuilder.ToDatabaseSummaryInfo();
            }
        }

        DatabaseSummaryInfoBuilder _summaryInfoBuilder;

        IDictionary _customPropTable;

        Database _db;

        Dictionary<string, string> _customProperties;

        Dictionary<string, string> _constProperties;
    }


    public static class Ext
    {
        //https://adn-cis.org/forum/index.php?topic=9374.msg39381#msg39381

        /// <summary>
        /// Запись в свойства чертежа нового свойства или изменение старого
        /// сохраняет все имеющиеся свойства чертежа
        /// Document должен быть заблокирован
        /// </summary>
        /// <param name="KeyProp">имя свойства. недопустимы двоеточия</param>
        /// <param name="ValProp">значение свойства. будет преобразовано в строку</param>
        /// <returns>успех</returns>
        internal static bool SetDrawingProperty(this Database db, string KeyProp, string ValProp)
        {
            if (db == null || string.IsNullOrEmpty(KeyProp) || ValProp == null) return false;
            DatabaseSummaryInfoBuilder ib = new DatabaseSummaryInfoBuilder(db.SummaryInfo);

            IDictionary customPropTable = ib.CustomPropertyTable;

            if (customPropTable.Contains(KeyProp))
            {
                customPropTable[KeyProp] = ValProp;
            }
            else
            {
                customPropTable.Add(KeyProp, ValProp);
            }

            db.SummaryInfo = ib.ToDatabaseSummaryInfo();
            return true;
        }

        internal static Dictionary<string, string> GetDrawingProperty(this Database db)
        {
            if (db == null) return null;

            Dictionary<string, string> customProperties = new Dictionary<string, string>();






            return customProperties;
        }
    }

    internal enum constProp
    {
        None,
        Title,
        Subject,
        RevisionNumber,
        LastSavedBy,
        Keywords,
        HyperlinkBase,
        Comments,
        Autor,
    }

}
