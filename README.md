# Collection of nanoCAD API bugs

## OpenDwg
тест открыть закрыть файлы средствами `Multicad`, `AutoCAD` vs `nanoCAD` ( *Teigha*)
- CommandTG.cs читатель Teigha
- CommandMC.cs читатель Multicad
## nanoCAD.Samples.NET
- FileOpenCloseCmd.cs - пакетное открытие в граф редакторе \
(nanoCAD 25 падает после пакетной обработки файлов в графическом редакторе, при таком открытии)
- DocPropCmd.cs - чтение/запись свойств dwg


