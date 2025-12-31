using System.ComponentModel;
using System.Diagnostics;
using static dRz.Test.OpenDwg.ServicesTG;
using System;
using Multicad.DatabaseServices;



#if NC
using Teigha.DatabaseServices;
using Teigha.Runtime;
using App = HostMgd.ApplicationServices;
using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using Db = Teigha.DatabaseServices;

#elif AC
using Db = Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.Runtime;
using App = Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

#endif


namespace dRz.Test.OpenDwg
{
    public partial class CommandTGMCS
    {

        /// <summary>
        /// смесь армяна с делаваром
        /// открытие файлов в цикле в Тайге переключение на рабочую базу корявый переброс в мультикад
        /// на некоторых файлах в нано в мультикад прилетает null
        /// в АК проверил на файлах которые не смог открыть нана, все ок, полный тест не делал, незачем
        /// нк 4к файлов Total 4000, Read 0, Err 6: time 00:06:43.1897441, 6 ошибок в мультикад нулл
        /// </summary>
        [CommandMethod("тдтмп")]
        [Description("открытие файлов в цикле в Тайге переключение рабочей базы чертежа, переоткрытие файла в мультикад")]
        public static void TGMCS()
        {
            Document doc = App.Application.DocumentManager.MdiActiveDocument;
            if (doc == null)
            {
                return;
            }

            Editor ed = doc.Editor;

            Stopwatch stw = new Stopwatch();

            string folder = Services.Browser();
            string[] files = Services.GetFilesOfDir(folder, true);

            string sender = CallerName(files.Length);

            Logger logger = new Logger($"{sender}");

            Logger loggerErr = new Logger($"{sender} ERR");

            logger.Log($"Total {files.Length} files");

            ed.WriteMessage($"Teigha: Total {files.Length} files");

            stw.Restart();

            int counter = 0;
            int reading = 0;
            int errors = 0;

            foreach (string file in files)
            {
                counter++;
                logger.Log($"{counter} Opening {file}");
                McDocument mcDocument = McDocumentsManager.GetDocument(file);

                if (mcDocument == null)  //проверка на нулл, если нулл то пропуск и записать в лог, что файл пропущен
                {
                    try
                    {
                        using (Database extDBase = new Database(false, true))
                        {

                            extDBase.ReadDwgFile(file, Db.FileOpenMode.OpenForReadAndAllShare, false, "");//получаем базу чертежа


                            using (WorkingDatabaseSwitcher dbSwitcher = new WorkingDatabaseSwitcher(extDBase))
                            {
                                if (extDBase != null)
                                {
                                    mcDocument = McDocumentsManager.GetDocument(file);//перекидываем базу чертежа в мультикад 

                                    if (mcDocument == null)  //проверка на нулл, если нулл то пропуск и записать в лог, что файл пропущен
                                    {
                                        errors++;
                                        loggerErr.Log($"{errors} NULL >> {file} >>");

                                        ed.WriteMessage($"NULL >> {file} >> \n");
                                        continue;
                                    }
                                    else
                                    {
                                        //  тут работаем с мультикад документом
                                        reading++;
                                        logger.Log($"\t\tWorking {file}");

                                        mcDocument.Close();
                                        mcDocument.Dispose();
                                    }
                                }
                                else//тайга не смогла открыть эксепшен не выбросил, вряд ли сюда попадем, но мало ли
                                {
                                    errors++;
                                    logger.Log($"{counter} Not Open {file}");
                                }

                            }
                        }
                    }
                    catch (System.Exception ex)
                    {

                        errors++;
                        loggerErr.Log($"{errors} {file} error : {ex.Message} >> {ex.StackTrace}\n");


                        ed.WriteMessage($"\n{file} error : {ex.Message} >> {ex.StackTrace}\n");
                    }
                }
                else//чертеж уже был открыт
                {
                    reading++;
                    logger.Log($"\t\tWorking {file}");

                }

                logger.Log($"\t\tClosed {file}");
            }

            stw.Stop();


            string elapsedTime = stw.Elapsed.ToString();

            logger.Log($"Total {files.Length}, Read {reading}, Err {errors}: time {elapsedTime}", 1);

            ed.WriteMessage($"Teigha: Total {files.Length}, Read {reading}, Err {errors}: time {elapsedTime}");

            //вызов сборщика мусора сомнительно но пусть пока будет
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }



    }
}
