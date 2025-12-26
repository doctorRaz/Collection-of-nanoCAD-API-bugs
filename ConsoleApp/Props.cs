using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Props
    {

       internal void TestProps()
        {
            constProp cs = constProp.Title;
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
}
