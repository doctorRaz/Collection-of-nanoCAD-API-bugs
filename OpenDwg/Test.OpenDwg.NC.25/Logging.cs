using HostMgd.ApplicationServices;
using HostMgd.EditorInput;
using NLog;
using NLog.Targets;

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
