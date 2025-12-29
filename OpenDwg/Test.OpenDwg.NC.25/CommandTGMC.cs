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
    public partial class CommandTGMC
    {

        /// <summary>
        /// смесь армяна с делаваром
        /// открытие файлов в цикле в Тайге корявый переброс в мультикад
        /// на некоторых файлах в нано в мультикад прилетает null
        /// в АК проверил на файлах которые не смог открыть нана, все ок, полный тест не делал, незачем
        /// нк 4к файлов Total 4000, Read 0, Err 6: time 00:06:43.1897441, 6 ошибок в мультикад нулл
        /// </summary>
        [CommandMethod("тдм0")]
        [Description("открытие файлов в цикле в Тайге")]
        public static void TGMC()
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
                                    logger.Log($"\t\tWorking {file}");

                                    mcDocument.Close();
                                    mcDocument.Dispose();


                                }
                            }



                            //using (WorkingDatabaseSwitcher dbSwitcher = new WorkingDatabaseSwitcher(extDBase))
                            //{

                            //    reading++;
                            //    // …
                            //}
                        }
                    }
                    catch (System.Exception ex)
                    {

                        errors++;
                        loggerErr.Log($"{errors} {file} error : {ex.Message} >> {ex.StackTrace}\n");


                        ed.WriteMessage($"\n{file} error : {ex.Message} >> {ex.StackTrace}\n");
                    }
                }

                logger.Log($"\t\tClosed {file}");
            }

            stw.Stop();


            string elapsedTime = stw.Elapsed.ToString();

            logger.Log($"Total {files.Length}, Read {reading}, Err {errors}: time {elapsedTime}", 1);

            ed.WriteMessage($"Teigha: Total {files.Length}, Read {reading}, Err {errors}: time {elapsedTime}");

            //GC.Collect();//todo чистим за собой


            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }



    }
}
