using dRz.Test.OpenDwg;
using System.Text.RegularExpressions;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Logger logger = new Logger(0);


            int count = 10;
            logger.Log($"Total {count.ToString()} files");

            int i = 0;
            for (; i < count; i++)
            {
                logger.Log($"{i}\t\tWorking c:\\dddd\\dddd\\{i}");
            }

            logger.Log($"Teigha\tfiles {count}: time 10:10",1);
            constProp cs = constProp.Title;

            Console.WriteLine( $"The END" );
            Console.ReadKey();
             return;
            var ss = cs.ToString();

            Dictionary<string, string> customProperties = new Dictionary<string, string>()
         {
             //{"prop10<>/\":;?*|,='Ok<>/\":;?*|,='Ok", "val1"},
             {"test\nttt", "va\nl2"},
             //{"", "val2"},
             //     {"prop3", "val3"},
             //{"prop4", "val3"},
             //{"prop40", ""},
         };
            //bool bb = customProperties.ContainsKey("pro p4");

            //var rr = customProperties["prop4"];

            foreach (var item in customProperties)
            {


                string key = item.Key;
                if (string.IsNullOrWhiteSpace(key))
                {
                    key = "_";
                }

                string pattern = @"[\<\>\/\:\\\"";\?\*\|\,\=\']";
                key = Regex.Replace(key, pattern, "_");
                key = Regex.Replace(key, @"_+", "_");

                var vkey = key.Split("\n");

                var rkey = Regex.Replace(key, "\n", "<<###>>");
                Console.WriteLine(rkey);
                var rr = Regex.Replace(rkey, "<<###>>", "\n");
                Console.WriteLine(rr);

                key = string.Join("###", vkey);

                Console.WriteLine($"{item.Key}:\t{key} ->{item.Value}");
            }

        }
    }

    internal enum constProp
    {
        None,
        Title,
        Subject,
        RevisionNumber,
        LastSavedBy,
        Keywords,
        HyperlinkBase,
        Comments,
        Autor,
    }
}
