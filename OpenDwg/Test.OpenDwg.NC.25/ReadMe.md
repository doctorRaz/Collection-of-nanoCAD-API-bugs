# nanoCAD 23.1 vs 26 (.NET6.0, nuget 23.1.6324.4405)

> [!Note]
> изменения в API 26 не используются

## Батл нового nanoCAD 26 vs nanoCAD 23.1 


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
> В нано 23.1 бага: первый запуск аддона в сеансе нана всегда не может открыть файлы сыплет ошибками (открывает всего 5 файлов)
> второй запуск аддона нана работает Ок (если при первом не упадет),\
> возможно что то не прогружается или мой код недостаточно хорош? \
> т.е. этот момент учитываем и игнорируем


### 1к файлов

1. Использованные ресурсы перед тестом, аддон загружен\
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/перед_тестом_тд_загружен.png)

1. MC - открыть файл в `MultiCad`, закрыть\
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/MC.png)

1. MCD - открыть файл в `MultiCad`, закрыть, dispose\
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/MCD.png)

1. TG -  открыть файл в `Teigha`, закрыть\
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/TG.png)

1. TGMC - открыть файл в `Teigha`, получить открытый в `MultiCad`, закрыть\
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/TGMC.png)

### 4к файлов

1. TGMC - открыть файл в `Teigha`, получить открытый в `MultiCad`, закрыть
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/TGMG_4000.png)

1. MCD - открыть файл в `MultiCad`, закрыть, dispose\
> [!Note]
> nannoCAD26 на 3572 файле завис напрочь, тест не завершен 

![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/MCD_4000.png)


> [!IMPORTANT]
> на скринах видно, что по всей видимости в nanoCAD26 есть проблемы с утечкой памяти.\
> несмотря на это в целом nanoCAD26 мне показался более стабильным, если не требуется обрабатывать тысяци файлов


### Результаты


> [!WARNING]
> Заготовка


|CAD|Reader|Всего|Прочитано|Ошибок|Время|Примечание|
|---|---|---|---|---|---|---|
|23.1|Teigha|4000|4000|0|00:08:17.3445218|TG|
|26|Teigha|4000|4000|0|<b>00:06:19.6759072</b> |TG|
|AutoCAD|MultiCad|4000|4000|0|<b>00:04:39.5789037</b>|MC|
|AutoCAD|MultiCad|4000|4000|0|<b>00:04:50.1422993</b>|MCDI|
|nanoCAD|MultiCad|4000|4000|0|00:28:25.9152691|MCDI|
|nanoCAD|Teigha->MultiCad|4000|3994|6|00:06:43.1897441|TGMC 6 файлов не открыл|





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