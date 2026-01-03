# Collection of nanoCAD API bugs

## OpenDwg
тест открыть закрыть файлы средствами `Multicad`, `AutoCAD` vs `nanoCAD` ( *Teigha*)
- CommandTG.cs читатель Teigha
- CommandMC.cs читатель Multicad
- ...

[AutoCAD2025 vs nanoCAD 26.0](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/tree/main/OpenDwg#opendwg)\
[nanoCAD23.1 vs nanoCAD26.0](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/tree/main/OpenDwg/Test.OpenDwg.NC.25#nanocad-231-vs-26-net60-nuget-23163244405)

[Проблемные файлы](https://github.com/doctorRaz/Collection-of-nanoCAD-API-bugs/blob/main/OpenDwg/Example_Dwg/ReadMe.md#проблемные-файлы)



---

## nanoCAD.Samples.NET
- FileOpenCloseCmd.cs - пакетное открытие в граф редакторе \
(nanoCAD 25 падает после пакетной обработки файлов в графическом редакторе, при таком открытии)
- DocPropCmd.cs - чтение/запись свойств dwg


