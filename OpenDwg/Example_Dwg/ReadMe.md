На этих файлах в nanoCAD, такая конструкция 
`mcDocument = McDocumentsManager.GetDocument(file);` всегда возвращает null

в AutoCAD возвращает `McDocument`
```csharp
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
```

часть файлов получена экспортом из SIMARIS design, часть уже не помню как

---

если эти файлы открыть в редакторе (Open), то `McDocumentsManager.GetDocument(file)` вернет  документ
