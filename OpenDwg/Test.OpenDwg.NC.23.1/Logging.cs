using NLog;
using NLog.Targets;

#if NC||NC26
using HostMgd.ApplicationServices;
using HostMgd.EditorInput;

#elif AC

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;

#endif

namespace MyPlugin.Logging
{

    /// <summary>
    /// Кастомный Target
    /// </summary>
    /// <seealso cref="NLog.Targets.TargetWithLayout" />
    [Target("AutoCadCommandLine")]
    public class AutoCadCommandLineTarget : TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            var doc = Application.DocumentManager.MdiActiveDocument;
            if (doc == null) return;

            Editor ed = doc.Editor;
            string message = Layout.Render(logEvent);

            ed.WriteMessage("\n" + message);
        }
    }
}
