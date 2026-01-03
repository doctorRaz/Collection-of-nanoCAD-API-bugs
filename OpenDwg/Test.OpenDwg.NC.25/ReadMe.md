# nanoCAD 23.1 vs 26 (.NET6.0, nuget 23.1.6324.4405)

> [!TIP]
> изменения в API 26 не используются

## Батл нового nanoCAD 26 vs nanoCAD 23.1 

> [!Note]
> Результаты тестов получены на моем наборе файлов и конфигурации ПК.\
> Исходный код доступен для скачивания и модификации.\
> Очевидно, что Ваши результаты будут отличаться, но глобально ни по стабильности ни по утечкам памяти, ни по быстродействию, уверен ничего сильно не изменится. 


Как тестировал
---

- nanoCad`ы работают одновременно
- nanoCad`ы не перезагружаются
- тест ведется на реальных файлах, файлы не повторяются
- первые тесты на 1к файлов, смотрим общую производительность (время выполнения) и использование ресурсов
- последующие тесты 4 к файлов все то же что и выше и стабильность
- тест ведется пока один из кадов не упадет, зависнет сам или повесит систему

> [!Note]
> Первый скрин перед тестом, свезежагруженные кады
> дальше скрины после выполнения соответствующего теста

> [!WARNING]
> В нано 23.1 бага: первый запуск аддона в сеансе нана всегда не может открыть файлы сыплет ошибками \
> (открывает всего 5 файлов)
> второй запуск аддона нана работает (если при первом не упал),\
> возможно что то не прогружается или мой код недостаточно хорош? \
> т.е. этот момент учитываем и игнорируем


### 1к файлов

1. Использованные ресурсы перед тестом, аддон загружен

![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/перед_тестом_тд_загружен.png)

1. MC 
- открыть файл в *MultiCad*
- закрыть

![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/MC.png)

1. MCD 
- открыть файл в *MultiCad*
- закрыть
- dispose

![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/MCD.png)

1. TG 
- открыть файл в *Teigha* `extDBase.ReadDwgFile(file, Db.FileOpenMode.OpenForReadAndAllShare, false, "")`
- закрыть

![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/TG.png)

1. TGMC 
- открыть файл в *Teigha* `extDBase.ReadDwgFile(file, Db.FileOpenMode.OpenForReadAndAllShare, false, "")`
- получить открытый в *MultiCad* `mcDocument = McDocumentsManager.GetDocument(file);`
- закрыть

![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/TGMC.png)

### 4к файлов

1. TGMC 
- открыть файл в *Teigha* `extDBase.ReadDwgFile(file, Db.FileOpenMode.OpenForReadAndAllShare, false, "")`
- получить открытый в *MultiCad* `mcDocument = McDocumentsManager.GetDocument(file);`
- закрыть

![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/TGMG_4000.png)

1. MCD 
- открыть файл в *MultiCad*
- закрыть
- dispose

> [!Note]
> nannoCAD26 на 3572 файле завис напрочь, тест не завершен 

![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/MCD_4000.png)


> [!IMPORTANT]
> на скринах видно, что по всей видимости в nanoCAD26 есть проблемы с утечкой памяти.\
> несмотря на это в целом nanoCAD26 мне показался более стабильным, если не требуется обрабатывать тысячи файлов


### Результаты

|CAD|Reader|Всего|Прочитано|Ошибок|Время|Примечание|
|---|---|---|---|---|---|---|
|23.1|MultiCad|1000|1000|0|00:02:46.5932199|MC|
|26.0|MultiCad|1000|1000|0|00:02:56.1310068|MC|
|26.0|MultiCad|1000|1000|0|00:03:06.0906721|MCD|
|23.1|MultiCad|1000|1000|0|00:02:54.5055909|MCD|
|23.1|Teigha|1000|1000|0|00:00:51.3800222|TG|
|26.0|Teigha|1000|1000|0|00:01:46.0101870|TG|
|23.1|Teigha→MultiCad|1000|1000|0|00:00:55.2368675|TGMC|
|26.0|Teigha→MultiCad|1000|1000|0|00:01:47.5935899|TGMC|
|---|---|---|---|---|---|---|
|23.1|Teigha→MultiCad|4000|4000|0|00:04:17.9052528|TGMC|
|26.0|Teigha→MultiCad|4000|4000|0|00:07:52.1370046|TGMC|
|23.1|MultiCad|4000|4000|0|00:27:43.8395370|MCD|
|26.0|MultiCad|4000|3572|-|-|MCD , завис|

---


### MCD vs MCDI nc23.1 на автозаполнялка.dwg (5к одинаковых файлов)

мои персональные тараканы

> [!WARNING]
> нано 23,1 бага: первый запуск в нк23,1 всегда не может прочитать средствами мультикад сыплет ошибками
> второй запуск работает Ок, возможно что то не прогружается (наномодуль?)


- MCD -  `McDocument mcDocument;` -  вне цикла `mcDocument.Dispose()`
- MCDI -  `McDocument mcDocument; ` - внутри цикла `mcDocument.Dispose()`
- MC -  `McDocument mcDocument; ` - внутри цикла, файлы не диспозим


|CAD|Reader|Всего|Прочитано|Ошибок|Время|Примечание|
|---|---|---|---|---|---|:---:|
|nc23.1|MultiCad|5000|5000|0|000:16:15.5516999|MCD|
|nc23.1|MultiCad|5000|5000|0|00:16:07.1505257|MCDI|
|nc23.1|MultiCad|1000|1000|0|00:02:33.5982096|MCD|
|nc23.1|MultiCad|1000|1000|0|00:02:34.2624314|MCDI|
|nc23.1|MultiCad|1000|1000|0|00:02:35.0562535|MC|