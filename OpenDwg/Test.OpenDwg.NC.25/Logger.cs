using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace dRz.Test.OpenDwg
{
    public class Logger
    {
        public Logger(int dateSpan, string appName = @"logger")
        {

            Init(appName);

            _dateSpan = dateSpan;

            LogDelToDate();

        }

        public Logger(string appName = @"logger")
        {
            Init(appName);
        }


        void Init(string appName)
        {
            string date = DateTime.Now.ToString("yyyyMMdd-HH_mm_ss", CultureInfo.InvariantCulture);

            _appName = $"{date}_{appName}.log";

        }

        public async void Log(string message, [CallerMemberName] string caller = null)
        {

            string date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.FFFFF", CultureInfo.InvariantCulture);

            string text = $"{date}: {caller} {message}";

            // полная перезапись файла 
            using (StreamWriter writer = new StreamWriter(_path, true, Encoding.UTF8))
            {
                await writer.WriteLineAsync(text);

            }
        }

        public void Log(string message, int rowNumber, [CallerMemberName] string caller = null)
        {

            List<string> lst = new List<string>();
            using (StreamReader sreader = new StreamReader(_path))
            {
                while (!sreader.EndOfStream)
                {
                    lst.Add(sreader.ReadLine());
                }

            }

            string date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.FFFFF", CultureInfo.InvariantCulture);

            string text = $"{date}: {caller} {message}";

            //lst[0] = text;

            lst.Insert(rowNumber, text);

            using (StreamWriter writer = new StreamWriter(_path, false, Encoding.UTF8))
            {
                foreach (string line in lst)
                {
                    writer.WriteLine(line);
                }
            }
        }
                
        void LogDelToDate()
        {
            FileInfo[] files = new DirectoryInfo(Path.GetDirectoryName(_path)).GetFiles("*.LOG");
            foreach (FileInfo file in files)
            {
                if (DateTime.UtcNow - file.LastWriteTimeUtc > TimeSpan.FromDays(_dateSpan))
                {
                    try
                    {
                    File.Delete(file.FullName);
                    Console.WriteLine($"{file.LastWriteTimeUtc/*CreationTimeUtc*/} deleted:\t{file.Name}");

                    }
                    catch { }
                }
            }

        }



        string _appName = "";

        int _dateSpan = 6;
        private string _path => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _appName);
    }
}
