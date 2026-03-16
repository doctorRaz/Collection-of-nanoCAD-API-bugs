using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Teigha.Geometry;
using Teigha.Runtime;


//(command "-INSERT" "c:\\temp\\a6df.dwg" "0,0,0" "1" "1" "0")

namespace dRz.nanoCADbugs
{
    public partial class ClipboardInserter
    {
        // Команда для NanoCAD
        [CommandMethod("NC_PASTECLIPDWG")]
        public void PasteClipboardDwg()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;

            try
            {
                string dwg = FindClipboardDwg();

                if (dwg == null)
                {
                    ed.WriteMessage("\nDWG из буфера не найден.");
                    return;
                }

                Point3d pt = AskInsertPoint(ed);

                ExecuteInsert(doc, dwg, pt);

                ExecuteExplodeLast(doc);
            }
            catch (System.Exception ex)
            {
                ed.WriteMessage($"\nОшибка: {ex.Message}");
            }
        }

        private string FindClipboardDwg()
        {
            string temp = Path.GetTempPath();

            FileInfo? file = new DirectoryInfo(temp)
                .GetFiles("*.dwg")
                .OrderByDescending(f => f.LastWriteTime)
                .FirstOrDefault();

            return file?.FullName;
        }

        private Point3d AskInsertPoint(Editor ed)
        {
            PromptPointResult p = ed.GetPoint("\nУкажите точку вставки: ");

            if (p.Status != PromptStatus.OK)
            {
                throw new OperationCanceledException();
            }

            return p.Value;
        }

        private void ExecuteInsert(Document doc, string dwg, Point3d pt)
        {
            string p = FormatPoint(pt);

            dwg = dwg.Replace("\\", "\\\\");

            string cmd =
                "-INSERT\n" +
                $"{dwg}\n" +
                $"{p}\n" +
                "1\n" +
                "1\n" +
                "0\n";

            doc.SendStringToExecute(cmd, true, false, false);

        }

        private void ExecuteExplodeLast(Document doc)
        {
            doc.SendStringToExecute("EXPLODE L\n", true, false, false);
        }

        private string FormatPoint(Point3d pt)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0},{1},{2}",
                pt.X, pt.Y, pt.Z);
        }

    }
}