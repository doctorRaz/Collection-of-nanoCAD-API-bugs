using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Services
    {

        internal static string CallerName(int CountFiles/*, [CallerMemberName] string caller = null*/)
        {
            Version version =new Version(1,1,1);

            string appProductName = "app";// System.Windows.Forms.Application.ProductName;

            return $"{appProductName}_{version.Major.ToString()}.{version.Minor.ToString()}_{CountFiles.ToString()}";

        }

    }
}
