# Collection of nanoCAD API bugs

1. **OpenDwg** тест открыть закрыть файлы средствами `Multicad`, `AutoCAD` vs `nanoCAD` ( *Teigha*)
    - CommandTG.cs читает Teigha
	- CommandMC.cs читает Multicad
1. **nanoCAD.Samples.NET**
    - FileOpenCloseCmd.cs `drz_FileOpenClose` пакетное открытие в граф редакторе \
(nc25 падает после пакетной обработки файлов в графическом редакторе)
    - DocPropCmd.cs чтение свойств dwg


