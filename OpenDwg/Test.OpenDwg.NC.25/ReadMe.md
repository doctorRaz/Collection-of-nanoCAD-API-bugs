# nanoCAD 23.1-26

## .NET6.0







### Батл нового nanoCAD 26 vs nanoCAD 23.1 

nanoCad`ы работают одновременно \

#### в работе 1к файлов

- Использованные ресурсы перед тестом, аддон загружен\
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/перед_тестом_тд_загружен.png)

- - MC - открыть файл в `MultiCad`, закрыть\
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/MC.png)

- MCD - открыть файл в `MultiCad`, закрыть, dispose\
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/MCD.png)

- TG -  открыть файл в `Teigha`, закрыть\
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/TG.png)

- TGMC - открыть файл в `Teigha`, получить открытый в `MultiCad`, закрыть\
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/TGMC.png)

#### 4к файлов

- TGMC - открыть файл в `Teigha`, получить открытый в `MultiCad`, закрыть
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/TGMG_4000.png)

- MCD - открыть файл в `MultiCad`, закрыть, dispose\
-> [!Note]
> nannoCAD26 на 3572 файле завис напрочь, тест не завершен \
![memory](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/img/MCD_4000.png)


> [!WARNING]
> нано 23,1 бага: первый запуск в нк23,1 всегда не может открыть файлы сыплет ошибками
> второй запуск работает Ок (если при первом не упадет), возможно что то не прогружается (наномодуль?)
> уже неважно никто чинить не будет

> [!IMPORTANT]
> по всей видимости в nanoCAD26 есть проблемы с утечкой памяти.\
> несмотря на это целом nanoCAD26 мне показался более стабильным при работе с мультикад, \
> если мультикад не требуется то nanoCAD23 более предпочтителен, как более быстрый и стабильный



26
31-12-2025 17:53:32.35205: TGMC Total 11376, Read 11365, Err 11: time 00:24:05.9628150
сожрал 5 Гб  памяти, для дальнейшей работы без перезапуска непригоден



|CAD|Reader|Всего|Прочитано|Ошибок|Время|Примечание|
|---|---|---|---|---|---|---|
|AutoCAD|Teigha|4000|4000|0|00:08:17.3445218|TG|
|nanoCAD|Teigha|4000|4000|0|<b>00:06:19.6759072</b> |TG|
|AutoCAD|MultiCad|4000|4000|0|<b>00:04:39.5789037</b>|MC|
|AutoCAD|MultiCad|4000|4000|0|<b>00:04:50.1422993</b>|MCDI|
|nanoCAD|MultiCad|4000|4000|0|00:28:25.9152691|MCDI|
|nanoCAD|Teigha->MultiCad|4000|3994|6|00:06:43.1897441|TGMC 6 файлов не открыл|




походу утечка памяти в NC26 все же имеет место быть


---


### MCD vs MCDI nc23.1 на автозаполнялка.dwg (5к одинаковых файлов)

мои персональные тараканы

> [!WARNING]
> нано 23,1 бага: первый запуск в нк23,1 всегда не может прочитать средствами мультикад сыплет ошибками
> второй запуск работает Ок, возможно что то не прогружается (наномодуль?)
> уже неважно никто чинить не будет


- MCD -  `McDocument mcDocument; ` -- вне цикла
- MCDI -  `McDocument mcDocument; ` -- внутри цикла
- MC -  `McDocument mcDocument; ` -- внутри цикла, файл не диспозим по окончании, 23,1 фаталит на 7-м файле \
но можно запустить после предыдущих тестов


|CAD|Reader|Всего|Прочитано|Ошибок|Время|Примечание|
|---|---|---|---|---|---|:---:|
|nc23.1|MultiCad|5000|5000|0|000:16:15.5516999|MCD|
|nc23.1|MultiCad|5000|5000|0|00:16:07.1505257|MCDI|
|nc23.1|MultiCad|1000|1000|0|00:02:33.5982096|MCD|
|nc23.1|MultiCad|1000|1000|0|00:02:34.2624314|MCDI|
|nc23.1|MultiCad|1000|1000|0|00:02:35.0562535|MC|