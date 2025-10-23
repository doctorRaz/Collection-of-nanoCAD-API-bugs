using System.Text.RegularExpressions;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {

            constProp cs = constProp.Title;

            var ss=cs.ToString();

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

               var vkey = key.Split( "\n");

                var rkey = Regex.Replace(key, "\n", "###");
                Console.WriteLine(rkey);
                var rr = Regex.Replace(rkey, "###", "\n");
                Console.WriteLine(rr);

              key=  string.Join( "###",vkey);

                Console.WriteLine($"{item.Key}:\t{key} ->{item.Value}");
            }



            //string key = "test<>/\":;?*|,='Ok";
            //string pattern = @"[\<\>\/\:\\\"";\?\*\|\,\=\']";
            //key = Regex.Replace(key, pattern, "_");

            //string secondPattern = @"_+"; // Одно или более подчеркиваний

            //// Окончательная строка после второй замены
            //string finalResult = Regex.Replace(key, secondPattern, "_");

            //key = Regex.Replace(key, @"_+", "_");
            //Console.WriteLine(finalResult);
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
