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

namespace drz.Infrastructure.CAD.Services
{
    internal class EditorDocProp
    {

        public EditorDocProp()
        {
            doc = Cad.DocumentManager.MdiActiveDocument;

            LoaderDoc();
        }

        public EditorDocProp(App.Document _doc)
        {
            doc = _doc;

            LoaderDoc();
        }


        void LoaderDoc()
        {
            db = doc.Database;

            ib = new DatabaseSummaryInfoBuilder(db.SummaryInfo);

            customPropTable = ib.CustomPropertyTable;

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
                if (db == null) return null;

                Dictionary<string, string> customProperties = new Dictionary<string, string>();

                foreach (DictionaryEntry item in customPropTable)
                {
                    customProperties.Add(item.Key.ToString(), item.Value.ToString());
                }


                return customProperties;
            }
            set
            {
                if (db == null) return;

                foreach (KeyValuePair<string, string> item in value)
                {
                    if (customPropTable.Contains(item.Key))
                    {
                        customPropTable[item.Key] = item.Value;
                    }
                    else
                    {
                        customPropTable.Add(item.Key, item.Value);
                    }

                }

                db.SummaryInfo = ib.ToDatabaseSummaryInfo();
            }
        }

        DatabaseSummaryInfoBuilder ib;
        IDictionary customPropTable;
        App.Document doc;
        Database db;

        Dictionary<string, string> _customProperties;
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
}
