using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;

namespace dRz.Test.OpenDwg
{
    public class Logger
    {
        public Logger(string appName = @"logger")
        {
            string date = DateTime.Now.ToString("yyyyMMdd-HH_mm_ss",
                                CultureInfo.InvariantCulture);

            _appName = $"{date}_{appName}.log";

        }

        public void LogClear()
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
        }
        public async void Log(string message)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.FFFFF",
                                            CultureInfo.InvariantCulture);

            string text = $"{date}: {message}";

            // полная перезапись файла 
            using (StreamWriter writer = new StreamWriter(_path, true,Encoding.UTF8))
            {
                await writer.WriteLineAsync(text);

            }
        }

        string _appName = "";

        private string _path => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _appName);
    }
}
